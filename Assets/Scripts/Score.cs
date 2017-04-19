using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static Text scoreText;
    static public int score = 0;

    // Use this for initialization
    void Start () {
        score = 0;
	}
	
    static public void addScore() {
        score++;
    }

    static public void resetScore() {
        score = 0;
    }

    static public int getScore() {
        return score;
    }

    static public void setHighScore() {
        PlayerPrefs.SetInt("highscore", score);
    }

    static public int getHighScore() {
        return PlayerPrefs.GetInt("highscore");
    }

    static public void resetHighScore() {
        PlayerPrefs.SetInt("highscore", 0);
    }
}
