using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public GameObject coinVisual;

    static GameObject coinUI;

    static public int score = 0;

    // Use this for initialization
    void Start () {
        coinUI = coinVisual;
        score = 0;
	}
	
    static public void addScore() {
        coinUI.GetComponent<Animator>().SetBool("play", true);
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
