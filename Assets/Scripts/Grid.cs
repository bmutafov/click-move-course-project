using UnityEngine;

public class Grid :MonoBehaviour {

    public int gridSizeX;
    public int gridSizeZ;

    public int gridStartX = 0;
    public int gridStartZ = 0;

    static public Vector3 size;
    static public Vector3 start;
    // Use this for initialization
    void Start () {
        size.x = gridSizeX;
        size.y = 0;
        size.z = gridSizeZ;

        start.x = gridStartX;
        start.y = 0;
        start.z = gridStartZ;
    }
}
