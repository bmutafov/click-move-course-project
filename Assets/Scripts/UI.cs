using UnityEngine;

public class UI : MonoBehaviour {

    static private new Camera camera;

    void Start () {
        //Finding the maincamera GameObject
        camera = Camera.main;
    }

    /*
     * Moves UI element to stick over game Object
     * 
     * flaot x, float z
     * additional spacing on those axes
     * */


    static public void moveUIWithObject (GameObject toMove, Vector3 position, float x, float z) {
        Vector3 screenPos = camera.WorldToScreenPoint(position);
        screenPos = new Vector3(screenPos.x + x, screenPos.y + z, screenPos.z);
        toMove.GetComponent<RectTransform>().position = screenPos;
    }
}
