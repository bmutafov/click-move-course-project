using UnityEngine;

public class Barrier : MonoBehaviour {
	
	public enum Move {
        Forward,
        Backward,
        Static
    }

    public enum Orientation {
        Vertical,
        Horizontal
    };

    public Orientation orientation;
    public float speed;

    private Vector3 newPosition;
    private float addUneven = 0;
    private Move move;

    private void Start () {
        newPosition = transform.position;
        int size = (int) Mathf.Ceil(localScale());

        //if the size is even add 0.5 to fit the grid
        if (size % 2 == 0) addUneven = 0.5f;

        //starting movement is static
        move = Move.Static;
    }

    private void Update () {
        MoveBarrier();
    }

    private void OnMouseDrag () {
        Vector3 currentPosition = transform.position;
        
        RaycastHit hit = mouseClickPosition();

        //if mouseDrag position is > than the transform position (depending on orientation)
        //sets a move
        if (getOrientationAxis(hit.point) > getOrientationAxis(transform.position) + localScale() / 2) {
            move = Move.Forward;
        }

        //if mouseDrag position is < than the transform position (depending on orientation)
        //sets a move
        else if (getOrientationAxis(hit.point) < getOrientationAxis(transform.position) - localScale() / 2) {
            move = Move.Backward;
        }

        //if the mouse is on the barrier dont move the cube
        else {
            move = Move.Static;
        }
    }

    private void OnMouseUp () {
        move = Move.Static;
    }

    /*
     * Moves the barrier
     * Must be called every frame
     * */
    void MoveBarrier() {
        float step = Time.deltaTime;
        Vector3 oldPosition = transform.position;
        string stringZ = null, stringX = null;

        //generate a new position if there is a move wanted
        if (newPosition == transform.position && move != Move.Static) {

            //if move direction is forward set up the new position
            if (move == Move.Forward && getOrientationAxis(newPosition) < getOrientationAxis(Grid.size) - 2) {
                newPosition = transform.position + (getOrientationVector() * (Mathf.Ceil(localScale()) - 1));
            }

            //if move direction is backward set up the new direction
            if (move == Move.Backward && getOrientationAxis(newPosition) > 1) {
                newPosition = transform.position - (getOrientationVector() * (Mathf.Ceil(localScale()) - 1));
            }

            //strings to find the cube on the new position
            stringZ = fixedRound(newPosition.z, move);
            stringX = fixedRound(newPosition.x, move);

            //full new position cube name
            string newPosCubeName = stringZ + stringX;

            //find the the cube game object
            GameObject newPosCube = GameObject.Find(newPosCubeName);

            //if the new position is not free -> disallow the movement
            if (!HighlightController.isFree(newPosCube)) {
                newPosition = oldPosition;
                move = Move.Static;
            }

            //forbiding movement on the last row of the grid
            if (newPosition.z == Grid.size.z/2 + 1) {
                newPosition.z = Grid.size.z / 2;
            }
        }

        //actual move of the barrier
        transform.position = Vector3.MoveTowards(transform.position, newPosition, step * speed);
    }

    /*
     * Returns a RaycastHit of the object hit with 
     * a mouse click
     * */
    RaycastHit mouseClickPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);
        return hit;
    }
    

    /*
     * fixes the transform position
     * adds 0.5f to fit the grid if
     * size is even
     * */
    Vector3 fixPosition(Vector3 position) {
        Vector3 returnPosition = position; 
        if(addUneven == 0.5f) {
            if (isHorizontal()) returnPosition += new Vector3(addUneven, 0f, 0f);
            else returnPosition += new Vector3(0f, 0f, addUneven);
        }
        return returnPosition;
    }
    /*
     * Functions below:
     * -- Return depends on Orientation
     * */

    /*
     * Returns  localScale of the transfrom
     * gets X or Z size of transform
     * depending on orientation
     * */
    private float localScale () {
        return getOrientationAxis(transform.localScale);
    }

    /*
     * Returns the X axis of a Vector3 if orietnation is Horizontal
     * Returns the Z axis of a Vector3 if orientation is Vertical
     * */
    private float getOrientationAxis(Vector3 point) {
        if (isHorizontal()) return point.x;
        else return point.z;
    }

    /*
     * Returns vector3 direction depending on Orientation
     * Returns Vector3(1,0,0) if isHorizontal
     * Returns Vector3(0,0,1) if isVertical
     * */
    private Vector3 getOrientationVector () {
        if (isHorizontal()) return Vector3.right;
        else return Vector3.forward;
    }

    /*
     * rounds to Ceil or Floor depending on
     * the move direction
     * */
    private string fixedRound(float pos, Move move) {
        if (move == Move.Backward) return Mathf.Floor(pos).ToString();
        if (move == Move.Forward) return Mathf.Ceil(pos).ToString();
        else return pos.ToString();
    }

    /*
     * returns true if orientation is horizontal
     * false if not
     * */
    private bool isHorizontal() {
        return orientation == Orientation.Horizontal;
    }
}
