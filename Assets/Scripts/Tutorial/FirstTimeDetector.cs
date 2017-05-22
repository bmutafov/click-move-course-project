using UnityEngine;

public class FirstTimeDetector : MonoBehaviour {

    public GameObject firstTimeScreen;

	// Use this for initialization
	void Awake () {
		if(!PlayerPrefs.HasKey("hasPlayed")) {
            firstTimeScreen.SetActive(true);
            PlayerPrefs.SetInt("hasPlayed", 1);
        }
	}

}
