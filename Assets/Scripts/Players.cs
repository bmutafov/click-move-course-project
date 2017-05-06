using UnityEngine;

public class Players : MonoBehaviour {

    static public int count;

    private void Start () {
        count = transform.childCount;
    }
}
