using UnityEngine;

public class CoinRotate : MonoBehaviour {
    public float speed;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0f, Time.deltaTime * speed, 0f));
	}
}
