using UnityEngine;

public class Grid :MonoBehaviour {

    public int gridSizeX;
    public int gridSizeZ;

    static public Vector3 size;

    // Use this for initialization
    void Start () {
        size.x = gridSizeX;
        size.y = 0;
        size.z = gridSizeZ;
    }
}
