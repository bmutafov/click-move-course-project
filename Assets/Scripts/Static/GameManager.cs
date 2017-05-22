using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager :MonoBehaviour {

    [Header("Panels")]
    public Transform gameOverPanel;
    public Transform howToPlayPanel;
    public Transform pausePanel;
    public Transform fadePanel;
    public Transform optionsPanel;

    [Header("Buttons")]
    public Button musicEnabledButton;
    public Button musicDisabledButton;

    [Header("Audio")]
    public AudioSource backGroundMusic;
    public AudioSource pauseClick;
    public AudioSource restartClick;
    public AudioSource gameOverAudio;

    [Space]
    public float musicVolume = 0.315f;
    public Slider volumeSlider;

    static public Transform staticGameOverPanel;

    static public bool isGameOver = false;

    private bool playGameOverSound = true;
    private bool isMusicOn = true;

    private void Start () {
        isGameOver = false;
        staticGameOverPanel = gameOverPanel;
        fadePanel.gameObject.SetActive(true);
    }

    static public void gameOver() {
        isGameOver = true;
        staticGameOverPanel.gameObject.SetActive(true);
    }

    public void restartGame() {
        restartClick.Play();
        resumeGame();
        StartCoroutine(sceneFade("scene01"));
    }

    public void pauseGame() {
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
        backGroundMusic.Pause();
        pauseClick.Play();
    }

    public void resumeGame() {
        pausePanel.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
        backGroundMusic.UnPause();
    }

    public void quitGame() {
        Application.Quit();
    }

    public void hideHowToPlay() {
        howToPlayPanel.gameObject.SetActive(false);
    }

    public void increaseQuailty() {
        QualitySettings.IncreaseLevel(true);
    }

    public void decreaseQuailty () {
        QualitySettings.DecreaseLevel(true);
    }

    public void loadMainMenu() {
        Time.timeScale = 1;
        StartCoroutine(sceneFade("MainMenu"));
    }

    public void loadTutorial() {
        StartCoroutine(sceneFade("Tutorial"));
    }

    public void loadOptions() {
        pausePanel.gameObject.SetActive(false);
        optionsPanel.gameObject.SetActive(true);
    }

    public void loadPause() {
        optionsPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(true);
    }

    public void toggleMusic() {
        isMusicOn = !isMusicOn;
        if (isMusicOn) {
            backGroundMusic.volume = musicVolume;
            musicDisabledButton.gameObject.SetActive(false);
            musicEnabledButton.gameObject.SetActive(true);
        } else {
            backGroundMusic.volume = 0;
            musicDisabledButton.gameObject.SetActive(true);
            musicEnabledButton.gameObject.SetActive(false);
        }
    }

    public void setGlobalVolume() {
        AudioListener.volume = volumeSlider.value;
    }
    
    public IEnumerator sceneFade(string newScene) {
        fadePanel.GetComponent<Animator>().SetBool("fade", true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(newScene);
    }

    void Update() {
        //pausing game when clicking back button
        if (isGameOver && playGameOverSound) {
            backGroundMusic.Pause();
            gameOverAudio.PlayDelayed(0.1f);
            playGameOverSound = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseGame();
        }
    }
}
