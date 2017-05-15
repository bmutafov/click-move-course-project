using UnityEngine;

public class CoinRotate : MonoBehaviour {
    public float speed;

	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0f, Time.deltaTime * speed, 0f));
	}

    public void OnCollisionEnter() {
        //speed = 0;
        transform.Rotate(new Vector3(90f, 0f, 0f));
        transform.localScale = new Vector3(25f, 25f, 5f);
    }

}
