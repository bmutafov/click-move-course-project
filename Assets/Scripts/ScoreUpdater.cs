using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour {

    public Text displayText;
    public TextType textType;
    public string scoreMessage = "Score";

    private int currentScore;


    public enum TextType {
        Score,
        Highscore
    }

	void Start () {
        UpdateText();
	}

	void Update () {
        if (Score.getScore() != currentScore) UpdateText();
	}

    void UpdateText() {
        if (textType == TextType.Score) {

            currentScore = Score.getScore();
            displayText.text = scoreMessage + ": " + currentScore;
            if (currentScore > Score.getHighScore()) Score.setHighScore();

        } else if(textType == TextType.Highscore) {

            currentScore = Score.getHighScore();
            displayText.text = "Highscore: " + currentScore;

        }
    }
}
