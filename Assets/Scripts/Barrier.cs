using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {
	
	public enum Orientation {
		Vertical,
		Horizontal
	};
	
	/* Public vars
	 * 
	 * GridSize assign the GridSystem object
	 * or the object with HighlightController
	 * which stores the gridSizeX vars
	 * 
	 * speed -> dragging speed of barrier
	 * */

	public Transform GridSize;
	public float speed = 5;
	public Orientation orientation;

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
	private float gridSizeOrientation;


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
		if(orientation == Orientation.Horizontal) {
			gridSizeOrientation = GridSize.GetComponent<HighlightController> ().gridSizeX;
		} else {
			gridSizeOrientation = GridSize.GetComponent<HighlightController> ().gridSizeZ;
		}

		//sets the first new position of the transform to be the current position
		newPosition = transform.position;

		//if Object scale is .5 adds a 0.5 to the snapping to match the grid
		if (localScaleOrientation() == 0) {
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

	/*
	 * returns true if orientation is Horizontal
	 * returns false if orientation is Vertical
	 */
	bool isHorizontalOrientation() {
		return orientation == Orientation.Horizontal;
	}


	/*
	 * if orientation is Horizontal returns transform.position.x  
	 * if orientation is Vertical returns transform.position.z
	 */
	float transformPositionOrientation () {
		if(isHorizontalOrientation()) {
			return transform.position.x;
		} else {
			return transform.position.z;		
		}
	}
	
	/*
	 * if orientation is Horizontal returns the X of the hitPoint
	 * if orientation is Vertical returns the Z of the hitPoint
	 */
	
	float hitPointOrientation(Vector3 hit) {
		if(isHorizontalOrientation()) {
			return hit.x;
		} else {
			return hit.z;
		}
	}
	
	
	/*
	 * returns newPosition of the barrier depending on the orientation
	 */
	Vector3 newPositionOrienation(float newXorZ) {
		if(isHorizontalOrientation()) {
			return new Vector3(newXorZ, 0f, 0f);
		} else {
			return new Vector3(0f, 0f, newXorZ);
		}
	}
	
	
	/*
	 * localScaleOrientation()
	 * returns localScale % 2 
	 * localScale depends on orientation of the barrier
	 * if orientation is horizontal gets localScaleX
	 * if orientation is vertical gets localScaleZ
	 */
	public float localScaleOrientation() {
		float localScale;
		if(isHorizontalOrientation()) localScale = transform.localScale.x;
		else localScale = transform.localScale.z;
		
		return Mathf.Round (localScale) % 2;
	}

	//Called everytime we drag the barrier
	void OnMouseDrag() {
        //gets cursor position
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
        float newXPos;
        Physics.Raycast (ray, out hit, 100);
        
        //calculating new X position
        if (transformPositionOrientation() > Mathf.Round(hitPointOrientation(hit.point))) {
             newXPos = Mathf.Round(hitPointOrientation(hit.point)) + addVal;
        } else {
        	 newXPos = Mathf.Round(hitPointOrientation(hit.point)) - addVal;
        }

		/* newXpos >= 0 -> dont go out of the grid (left side)!
		 * newXpos < gridSizeX - 0.5f -addVal -> dont go out of the grid (right side) !
		 * */
		if(newXPos >= 0 && newXPos < gridSizeOrientation - 0.5f - addVal) {

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
        if (newXPos > transformPositionOrientation())
            return moveWanted.Right;
        else 
            return moveWanted.Left;
     
	}

	bool isFree(moveWanted move) {
		float newX = Mathf.Round(localScaleOrientation)*2 - 0.5f - addVal;
		if(move == moveWanted.Left) newX = -newX;
		float hitDistance = Mathf.Round(localScaleOrientation)/2 + 0.5f + addVal;
		bool result = Physics.Raycast(transform.position, newPositionOrienation(newX), hitDistance);
		return !result;
	}

	/* isRightFree()
	 * 
	 * raycasts to the right from the cubes center
	 * returns true if next grid position is free
	 * returns false if next grid position is taken by a cube/player
	 * 
	 * */
	bool isRightFree() {
        float newX = Mathf.Round(localScaleOrientation)*2 - 0.5f - addVal;
        float hitDistance = Mathf.Round(localScaleOrientation)/2 + 0.5f + addVal;
        bool result = Physics.Raycast(transform.position, newPositionOrienation(newX), hitDistance);
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
		float newX = -Mathf.Round(localScaleOrientation)*2 - 0.5f - addVal;
		float hitDistance = Mathf.Round(localScaleOrientation)/2 + 0.5f + addVal;
		bool result = Physics.Raycast(transform.position, newPositionOrienation(newX), hitDistance);
		return !result;
	}
}
