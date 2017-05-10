using UnityEngine;

public class Grid :MonoBehaviour {

    [Header("Grid")]
    public Vector3 gridSize;
    public Vector3 gridFirstBlock;

    [Header("Destinations")]
    public Vector3 destination;
    public Vector3[] bonusDestinations;

    static public Vector3 size;
    static public Vector3 start;
    static public Vector3[] bonusDest;
    
    // Use this for initialization
    void Start () {
        size = gridSize;
        start = gridFirstBlock;
        bonusDest = bonusDestinations;
        HighlightController.catchDestination = destination;
    }

    static public bool isBonusDestination(float x, float z) {
        foreach(Vector3 bonus in bonusDest) {
            if (bonus.x == x && bonus.z == z) return true;
        }
        return false;
    }
}
