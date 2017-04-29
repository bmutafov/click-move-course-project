using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {

	[Header("Scripting and Values")]
	public Transform HighlightController;
	public float speed = 4;

	[Space]

	[Header("Materials")]
	public Material normalMat;
	public Material selectedMat;
    public Material targetMat;

    [System.NonSerialized]
    public Vector3 newPosition;
    static public GameObject activePlayer;
    
	// Use this for initialization
	void Start () {
		newPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			//IF click hit a collider
			if(Physics.Raycast( ray, out hit, 100 )) {
                /* if we have hit a collider with mouse click
                * --
                * if the block is selected
                * &&
                *the hit object has tag Selected (means highlighted cube->available move)
                **/
                if (isSelected () && (hit.transform.tag == "Selected" || hit.transform.tag == "Catch") ) {
                    /* if the hit tag is Catch
                     * then check the Grid Cube under if has tag "Selected"
                     * or if the hit tag is Selected
                     * if true sets new position for the Player
                     * if not skips
                     * */
                    if ( (hit.transform.tag == "Catch" && GameObject.Find("" + Mathf.Round(hit.point.z) + Mathf.Round(hit.point.x)).transform.tag == "Selected")
                        || hit.transform.tag == "Selected") {
                        //give a new position to the player(cube), rounded so it matches grid
                        newPosition = new Vector3(Mathf.Round(hit.point.x), transform.position.y, Mathf.Round(hit.point.z));
                        deselectAll();
                    }
				}

                //if cube is selected and clicked object is a non-highlighted cube
                // == move is forbidden
                if (isSelected () && hit.transform.name != transform.name) {
					deselectAll ();
				}
			}
		}

        //Move the cube to the new position (same if no new click given)
        if (newPosition != transform.parent.position) {
            if (newPosition.z == Grid.size.z && transform.childCount < 2) {
                GetComponent<Animator>().SetBool("catch", true);
                newPosition.z -= 1;
            } else {
                moveCube(newPosition);
            }
        }

		//if cube is selected before it reaches its new position
		//highlights the path when it reaches it
		if (isSelected ())
			selectPath ();
	}

	//on click on the player object
	void OnMouseDown() {
		//if is currently selected
		if (isSelected()) {
			deselectAll ();  //deselect it
		} else {
			selectThis (); //select it
			selectPath ();
		}
	}

	void moveCube(Vector3 newPosition) {
        float step = Time.deltaTime * speed;
		transform.parent.position = Vector3.MoveTowards (transform.position, newPosition, step);
	}

	//isSelected() return true if cube is selected, false if not
	bool isSelected() {
		if (transform.tag == "Player")
			return false;
		return true;
	}
		
	//selects available PATH (see HighlightController)
	void selectPath() {
		HighlightController.GetComponent<HighlightController> ().findPath (transform);
	}

	//selects cube
	void selectThis() {
        activePlayer = transform.gameObject;
        Materials.set(transform.gameObject, selectedMat);
        transform.tag = "PlayerSelected";
	}

	void deselectAll() {
		deselectPath ();
		deselectThis ();
	}

	//deselects ALL SELECTED CUBES (see HighlightController)
	void deselectPath() {
		HighlightController.GetComponent<HighlightController> ().deselectAll ();
	}

	//deselects cube
	void deselectThis() {
        Material newMat;
        if (transform.name.Contains("Target")) {
            newMat = targetMat;
        } else {
            newMat = normalMat;
        }
        Materials.set(transform.gameObject, newMat);
        transform.tag = "Player";
	}
}
