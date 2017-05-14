using UnityEngine;

public class SetBool : MonoBehaviour {

    public string boolToSet;
    public bool value;

	void setBool() {
        transform.GetComponent<Animator>().SetBool(boolToSet, value);
    }
}
