using UnityEngine;
using UnityEngine.UI;

public class Catch : MonoBehaviour {

    public float timeToBring;
    public GameObject timeLeftPrefab;
    public Material playerMat;

    private Timer bringTimer;
    private Transform image;
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
        instObj.transform.SetSiblingIndex(2);
        /*
         * Getting references to image on the prefab
         * And the time left text
         * */
        image = instObj.transform;
        remainingTimeText = instObj.transform.GetChild(0).GetComponent<Text>();

        /* getting reference to the timer script */
        bringTimer = transform.GetComponent<Timer> ();
    }
	
	// Update is called once per frame
	void Update () {
        if (collided) {
            destroyObjectIfOnFinish();
            CatchSpawn.catchCubeName = null;
        }
        if(counterActive) {
            float timeleft = bringTimer.TimeRemaining();
            if(timeleft <= 0) {
                counterActive = false;
                destroyAll(true);
            }
            bringTimer.moveUIWithObject(image.gameObject, 50, 100, transform.position);
            remainingTimeText.text = bringTimer.secondsToMinutes(timeleft);
        }
    }

	/* OnColissionEnter
	 * collision -> returns the object which it has collided with
	 * */
	void OnCollisionEnter(Collision collision) {
		/* If its first collision
		 * */
		if (!collided && collision.transform.tag == "Player") {
            //deactivates the Dashed border on the catch position
            HighlightController.deactivateAllDashedBorders();


            transform.parent = collision.transform.parent; //make it a child of object it has collided with
            transform.SetAsLastSibling(); //sets the catch as last sibling in p1 gameobject
			transform.localPosition = new Vector3 (0f, 0.6f, 0f); //sets its local position to center

			collided = true; //no more collisions

			GetComponent<SphereCollider> ().enabled = false; //disable sphere colider for clicks.
            collision.transform.GetComponent<Animator>().SetBool("catch", false); //disables the animation of the cube


            /* get a reference to the last cube with the catch
             * make it a normal catch cube if the catch is cought
             * */
            GameObject makeNormalCube = GameObject.Find(CatchSpawn.catchCubeName);
            makeNormalCube.transform.tag = "CatchCube";

            /*
             * Start timer
             * */
            CatchSpawn.disableText = true;
            bringTimer.startTimer(timeToBring);
            counterActive = true;
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

                //getting the real cube reference
                Transform player = transform.parent.GetChild(0);

                //length of the player GameObject name
                int nameLength = transform.parent.GetChild(0).name.Length;
                
                //removing the last 6 simbols (TARGET)
                player.name = player.name.Substring(0, nameLength - 6);

                //destroying the catch
                destroyAll(false);

                //Adds Score and refreshes material
                Score.addScore();
                Materials.set(player.gameObject, playerMat);
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
