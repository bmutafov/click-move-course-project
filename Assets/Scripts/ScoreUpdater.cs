using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUpdater : MonoBehaviour {

    public Text displayText;
    public TextType textType;

    private int currentScore;


    public enum TextType {
        Score,
        Highscore
    }
	// Use this for initialization
	void Start () {
        UpdateText();
	}
	
	// Update is called once per frame
	void Update () {
        if (Score.getScore() != currentScore) UpdateText();
	}

    void UpdateText() {
        if (textType == TextType.Score) {
            currentScore = Score.getScore();
            displayText.text = "Score: " + currentScore;
            if (currentScore > Score.getHighScore()) Score.setHighScore();
        } else if(textType == TextType.Highscore) {
            currentScore = Score.getHighScore();
            displayText.text = "Highscore: " + currentScore;
        }
    }
}
