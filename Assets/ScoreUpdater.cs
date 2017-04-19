using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUpdater : MonoBehaviour {

    public Text displayText;
    private int currentScore;
	// Use this for initialization
	void Start () {
        UpdateText();
	}
	
	// Update is called once per frame
	void Update () {
        if (Score.getScore() != currentScore) UpdateText();
	}

    void UpdateText() {
        currentScore = Score.getScore();
        displayText.text = "Score: " + currentScore;
    }
}
