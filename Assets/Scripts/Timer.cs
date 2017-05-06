using UnityEngine;

public class Timer : MonoBehaviour {

    private float timeleft = 0;
    private bool isStarted = false;
    private bool isPaused = false;

    private new Camera camera;
    void Start() {
        //Finding the maincamera GameObject
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update () {    
        if (isStarted && !isPaused) runTimer();
	}

    private bool runTimer() {
        if (timeleft > 0) {
            timeleft -= Time.deltaTime;
            return true;
        }
        isStarted = false;
        return false;
    }

    public void startTimer(float timeleft) {
        if (!isStarted) {
            this.timeleft = timeleft;
            isStarted = true;
        }
    }

    public void restartTimer(float time) {
        timeleft = time;
        isPaused = false;
    }
    public  float TimeRemaining() {
        return timeleft;
    }

    public void resetTimer() {
        timeleft = 0;
        isStarted = false;
    }

    public void pauseTimer(bool state) {
        isPaused = state;
    }
    /*
     * gets time in seconds
     * returns a string
     * formatted in 0:00
     * */
    public string secondsToMinutes(float time) {
        time = Mathf.Round(time);
        string minutes, seconds;
        if (time > 59) {
            int intMinutes = (int)time / 60;
            minutes = intMinutes.ToString();
            seconds = (time - intMinutes * 60).ToString();
        }
        else {
            minutes = "0";
            seconds = time.ToString();
        }
        if (seconds.Length == 1) {
            seconds = "0" + seconds;
        }
        return minutes + ":" + seconds;
    }

    /*
     * Moves UI element to stick over game Object
     * 
     * flaot x, float z
     * additional spacing on those axes
      * */
    public void moveUIWithObject(GameObject toMove, float x, float z, Vector3 position) {
        Vector3 screenPos = camera.WorldToScreenPoint(position);
        screenPos = new Vector3(screenPos.x + x, screenPos.y + z, screenPos.z);
        toMove.GetComponent<RectTransform>().position = screenPos;
    }
}
