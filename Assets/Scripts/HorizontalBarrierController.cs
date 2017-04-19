using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBarrierController : MonoBehaviour {
	
	/* Public vars
	 * 
	 * GridSize assign the GridSystem object
	 * or the object with HighlightController
	 * which stores the gridSizeX vars
	 * 
	 * speed -> dragging speed of barrier
	 * */
	[Header("Tweak values")]
	public Transform GridSize;
	public float speed = 5;


	/*
	 * Private variables
	 * 
	 * grodSizeX -> stores the size of the Grid on the X axis
	 * newPosition -> stores the Vector3 value for the newPosition wanted
	 * addVal -> if the cube has an even scale on the X axis ..
	 * .. adds 0.5f to everything to fix its position on the grid
	 */
	private int gridSizeX;
	private Vector3 newPosition;
	private float addVal;


	/* Allowment variables
	 * 
	 * allowRight -> allows dragging right
	 * alowLeft -> allows dragging left
	 * 
	 * if true -> allowed
	 * if false -> dissallowed movement
	 * 
	 */
	private bool allowRightMovement = true;
	private bool allowLeftMovement = true;

	/*
	 * Defining moveWanted
	 */
	enum moveWanted {Right, Left, NoMove};

	// Use this for initialization
	void Start () {
		//Get the grid size from the HighlightController
		gridSizeX = GridSize.GetComponent<HighlightController> ().gridSizeX;

		//sets the first new position of the transform to be the current position
		newPosition = transform.position;

		//if Object scale is .5 adds a 0.5 to the snapping to match the grid
		if (Mathf.Round (transform.localScale.x) % 2 == 0) {
            addVal = 0.5f;
        } else {
			addVal = 0;
		}
	}

    // Update is called once per frame
    void Update () {
        if (newPosition != transform.position) {
            moveCube(newPosition);
            FPSController.setFps(30);
        } else FPSController.setFps(10);
    }

    //moving the barrier
    void moveCube(Vector3 newPosition) {
		float step = Time.deltaTime * speed;
		transform.position = Vector3.MoveTowards (transform.position, newPosition, step);
	}

	//Called everytime we drag the barrier
	void OnMouseDrag() {
        //gets cursor position
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
        float newXPos;
        Physics.Raycast (ray, out hit, 100);
        //calculating new X position
        if (transform.position.x > Mathf.Round(hit.point.x)) {
             newXPos = Mathf.Round(hit.point.x) + addVal;
        }else
         newXPos = Mathf.Round(hit.point.x) - addVal;

		/* newXpos >= 0 -> dont go out of the grid (left side)!
		 * newXpos < gridSizeX - 0.5f -addVal -> dont go out of the grid (right side) !
		 * */
		if(newXPos >= 0 && newXPos < gridSizeX - 0.5f - addVal) {

			/* allows and disallows movement
			 * left and right
			 * 
			 * depends on if is free
			 * or taken by another cube/player
			 * 
			 * */
			if (!isRightFree ()) {
				allowRightMovement = false;
			} else {
				allowRightMovement = true;
			}
			if (!isLeftFree ()) {
				allowLeftMovement = false;
			} else {
				allowLeftMovement = true;
			}

			//Get the wanted move direction
			moveWanted moveDirection = getMoveWanted (newXPos);

			/* moveDirection == moveWanted.Right && allowRightMovement
			 * --> if the wanted move direction is right and move to the right is allowed --> allow movement, set newPosition
			 * 
			 * or
			 * 
			 * moveDirection == moveWanted.Left && allowLeftMovement
			 * --> if wanted move derection is left and move to the left is allowed --> allow movement, set newPosition
			 * 
			 * else forbid movement, block stays on spot
			 * */
			if( moveDirection == moveWanted.Right && allowRightMovement
				|| moveDirection == moveWanted.Left && allowLeftMovement)
				newPosition = new Vector3 (newXPos, transform.position.y, transform.position.z);
		}
	}


	/* getMoveWanted
	 * Arguments: newXpos -> the flaot value of mouse cursor when dragging
	 * 
	 * returns enum Right if the mouse X position is greater than the cube's
	 * rutrns enum Left if not
	 * */
	moveWanted getMoveWanted(float newXPos) {
        if (newXPos > transform.position.x)
            return moveWanted.Right;
        else 
            return moveWanted.Left;
     
	}


	/* isRightFree()
	 * 
	 * raycasts to the right from the cubes center
	 * returns true if next grid position is free
	 * returns false if next grid position is taken by a cube/player
	 * 
	 * */
	bool isRightFree() {
        float newX = Mathf.Round(transform.localScale.x)*2 - 0.5f - addVal;
        float hitDistance = Mathf.Round(transform.localScale.x)/2 + 0.5f + addVal;
        bool result = Physics.Raycast(transform.position, new Vector3(newX ,0f, 0f), hitDistance);
		return !result;
	}

	/* isLeftFree()
	 * 
	 * raycasts to the left from the cubes center
	 * returns true if next grid position is free
	 * returns false if next grid position is taken by a cube/player
	 * 
	 * */
	bool isLeftFree() {
		float newX = -Mathf.Round(transform.localScale.x)*2 - 0.5f - addVal;
		float hitDistance = Mathf.Round(transform.localScale.x)/2 + 0.5f + addVal;
		bool result = Physics.Raycast(transform.position, new Vector3( newX, 0f ,0f), hitDistance);
		return !result;
	}
}
