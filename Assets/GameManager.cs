using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    public Transform gameOverPanel;
    static public Transform staticGameOverPanel;

    static public bool isGameOver = false;

    private void Start () {
        isGameOver = false;
        staticGameOverPanel = gameOverPanel;
    }

    static public void gameOver() {
        isGameOver = true;
        staticGameOverPanel.gameObject.SetActive(true);
    }

    public void restartGame() {
        SceneManager.LoadScene("scene01");
    }
}
