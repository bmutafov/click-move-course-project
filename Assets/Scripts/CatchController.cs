using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CatchController : MonoBehaviour {

    public float timeleft = 10;
    public GameObject timeLeftPrefab;

    private Transform image;
    private new Camera camera;
    private Text remainingTimeText;
    private bool collided = false;
    private bool counterActive = false;
	// Use this for initialization
	void Start () {
        /*Instantiating a image with timeleft
         * parent canvas ...
         * */
        GameObject canvas = GameObject.Find("Canvas");
        GameObject instObj = Instantiate(timeLeftPrefab, canvas.transform);

        //Finding the maincamera GameObject
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();

        /*
         * Getting references to image on the prefab
         * And the time left text
         * */
        image = instObj.transform;
        remainingTimeText = instObj.transform.GetChild(0).GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (collided) {
            destroyObjectIfOnFinish();
            CatchSpawnController.catchCubeName = null;
        }
        if(counterActive) {
            timer();
            moveUIWithObject(image.gameObject, 50, 100);
            string displayTime = "0:" + Mathf.Round(timeleft);
            if(Mathf.Round(timeleft) < 10) {
                displayTime = "0:0" + Mathf.Round(timeleft);
            }
            remainingTimeText.text = displayTime;
        }
    }

    /*
     * timer()
     * Timer function
     * Starts from whatever timeleft is set
     * Ends on 0
     * Returns true if timeleft > 0
     * Returns false if timeleft <= 0
     * */
    bool timer() {
        counterActive = true;
        timeleft -= Time.deltaTime;
        if (timeleft <= 0) {
            destroyAll(true);
            return false;
        }
        return true;
    }

    /*
     * Moves UI element to stick over game Object
     * 
     * flaot x, float z
     * additional spacing on those axes
     * */
    void moveUIWithObject(GameObject toMove, float x, float z) {
        Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
        screenPos = new Vector3(screenPos.x + x, screenPos.y + z, screenPos.z);
        toMove.GetComponent<RectTransform>().position = screenPos;
    }

	/* OnColissionEnter
	 * collision -> returns the object which it has collided with
	 * */
	void OnCollisionEnter(Collision collision) {
		/* If its first collision
		 * */
		if (!collided) {
			transform.parent = collision.transform; //make it a child of object it has collided with
			transform.localPosition = new Vector3 (0f, 0.6f, 0f); //sets its local position to center
			collided = true; //no more collisions
			GetComponent<SphereCollider> ().enabled = false; //disable sphere colider for clicks.


            /* get a reference to the last cube with the catch
             * make it a normal catch cube if the catch is cought
             * */
            GameObject makeNormalCube = GameObject.Find(CatchSpawnController.catchCubeName);
            makeNormalCube.transform.tag = "CatchCube";

            /*
             * Start timer
             * */
            timer();
		}
	}

    /*
     * Checks if Player has a child ( catch )
     * And is on Destination ( coordinates on Vector3 destination )
     * and destroys All if both true 
     * */

    void destroyObjectIfOnFinish () {
        RaycastHit hit;
        //if it has collided and there is something above the destroy possition
        Vector3 destination = HighlightController.catchDestination;
        if (Physics.Raycast(destination, Vector3.up, out hit)) {
            //if that is player and has a child in it 
            if (hit.transform.name.Contains("Target") && hit.transform.childCount > 0) {
                int nameLength = transform.parent.name.Length - 6;
                transform.parent.name = transform.parent.name.Substring(0, nameLength);
                destroyAll(false);
                Score.addScore();
                Materials.set(transform.parent.gameObject, "PlayerMat");
            }
        }
    }

    /*
     * Destroys every gameObject related to the catch
     * Including:
     * -> self GameObject
     * -> image GameObject
     * -> text GameObject
     * sets counterActive = false
     * */
    void destroyAll (bool gameOver) {
        Destroy(image.gameObject);
        Destroy(remainingTimeText.gameObject);
        counterActive = false;
        Destroy(gameObject);

        if (gameOver) {
            GameManager.gameOver();
        }
    }

}
