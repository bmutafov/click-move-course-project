using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public Transform gameOverPanel;
    public Transform howToPlayPanel;
    public Transform pausePanel;

    public Timer[] timers;

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

    public void pauseGame() {
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void resumeGame() {
        pausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void hideHowToPlay() {
        howToPlayPanel.gameObject.SetActive(false);
    }

}
