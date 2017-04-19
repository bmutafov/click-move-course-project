using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreDisplay : MonoBehaviour {

    public Text scoreText;

	// Use this for initialization
	void Start () {
        scoreText.text = "Your score: " + Score.score.ToString();
	}
}
