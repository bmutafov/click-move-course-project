using UnityEngine;

public class Grid :MonoBehaviour {

    [Header("Grid")]
    public Vector3 gridSize;
    public Vector3 gridFirstBlock;

    [Header("The destination for the catch")]
    public Vector3 destination;

    static public Vector3 size;
    static public Vector3 start;
    
    // Use this for initialization
    void Start () {
        size = gridSize;
        start = gridFirstBlock;
        HighlightController.catchDestination = destination;
    }
}
