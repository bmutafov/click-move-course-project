using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour {

	/* Public vars
	 * 
	 * GridSize assign the GridSystem object
	 * or the object with HighlightController
	 * which stores the gridSizeZ and gridSizeX vars
	 * 
	 * speed -> dragging speed of barrier
	 * */
	[Header("Tweak values")]
	public Transform GridSize;
	public float speed = 5;
    public Transform[] players;


	/*
	 * Private variables
	 * 
	 * grodSizeZ -> stores the size of the Grid on the Z axis
	 * newPosition -> stores the Vector3 value for the newPosition wanted
	 * addVal -> if the cube has an even scale on the Z axis ..
	 * .. adds 0.5f to everything to fix its position on the grid
	 */
	private int gridSizeZ;
	private Vector3 newPosition;
	private float addVal;


	/* Allowment variables
	 * 
	 * allowUp -> allows dragging upwards
	 * alowDown -> allows dragging down
	 * 
	 * if true -> allowed
	 * if false -> dissallowed movement
	 * 
	 */
	private bool allowUpMovement = true;
	private bool allowDownMovement = true;

	/*
	 * Defining moveWanted
	 * Up for Up movement
	 * Down for Down movement
	 */
	enum moveWanted {Up, Down};

	// Use this for initialization
	void Start () {
		//Get the grid size from the HighlightController and set it -1 ( to ignore the catch row ) 
		gridSizeZ = GridSize.GetComponent<HighlightController> ().gridSizeZ - 1;

		//sets the first new position of the transform to be the current position
		newPosition = transform.position;

		//if Object scale is .5 adds a 0.5 to the snapping to match the grid
		if (Mathf.Round (transform.localScale.z) % 2 == 0) {
			addVal = 0.5f;
		} else {
			addVal = 0f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
		Physics.Raycast (ray, out hit, 100);

		//calculating new Z position
		float newZPos = Mathf.Round (hit.point.z) - addVal;

		/* newZpos >= 0 -> dont go out of the grid (bottom side)!
		 * newZpos < gridSizeZ - 0.5f -addVal -> dont go out of the grid (top side) !
		 * */
		if(newZPos >= 0 && newZPos < gridSizeZ - 0.5f - addVal) {

			/* allows and disallows movement
			 * upwards and downwards
			 * 
			 * depends on if is free
			 * or taken by another cube/player
			 * 
			 * */
			if (!isUpFree ()) {
				allowUpMovement = false;
			} else {
				allowUpMovement = true;
			}
			if (!isDownFree ()) {
				allowDownMovement = false;
			} else {
				allowDownMovement = true;
			}

			//Get the wanted move direction
			moveWanted moveDirection = getMoveWanted (newZPos);

			/* moveDirection == moveWanted.Up && allowUpMovement
			 * --> if the wanted move direction is up and move upwards is allowed --> allow movement, set newPosition
			 * 
			 * or
			 * 
			 * moveDirection == moveWanted.Down && allowDownMovement
			 * --> if wanted move derection is down and move downwards is allowed --> allow movement, set newPosition
			 * 
			 * else forbid movement, block stays on spot
			 * */
			if(moveDirection == moveWanted.Up && allowUpMovement
				|| moveDirection == moveWanted.Down && allowDownMovement)
				newPosition = new Vector3 (transform.position.x, transform.position.y, newZPos);
		}
	}


	/* getMoveWanted
	 * Arguments: newZpos -> the flaot value of mouse cursor when dragging
	 * 
	 * returns enum Up if the mouse Z position is greater than the cube's
	 * rutrns enum DOwn if if not
	 * */
	moveWanted getMoveWanted(float newZPos) {
		if (newZPos > transform.position.z)
			return moveWanted.Up;
		else
			return moveWanted.Down;
	}


	/* isUpFree()
	 * 
	 * raycasts Upwards from the cubes center
	 * returns true if next grid position is free
	 * returns false if next grid position is taken by a cube/player
	 * 
	 * */
	bool isUpFree() {
        /*checking if grid space above is free (no other object above*/

		float newZ = Mathf.Round(transform.localScale.z)*2 - 0.5f - addVal;
		float hitDistance = Mathf.Round(transform.localScale.z)/2 + 0.5f + addVal;
		bool result = Physics.Raycast(transform.position, new Vector3(0f ,0f, newZ), hitDistance);

//        for(int i = 0; i < players.Length; i++) {
 //           Debug.Log(players[i].GetComponent<MoveController>().newPosition);
  //          if (isOnSameXZ(players[i].GetComponent<MoveController>().newPosition)) return false;
   //     }
		return !result;
	}

	/* isDownFree()
	 * 
	 * raycasts Downwards from the cubes center
	 * returns true if next grid position is free
	 * returns false if next grid position is taken by a cube/player
	 * 
	 * */
	bool isDownFree() {
		float newZ = -Mathf.Round(transform.localScale.z)*2 - 0.5f - addVal;
		float hitDistance = Mathf.Round(transform.localScale.z)/2 + 0.5f + addVal;
		bool result = Physics.Raycast(transform.position, new Vector3(0f ,0f, newZ), hitDistance);
		return !result;
	}
}
