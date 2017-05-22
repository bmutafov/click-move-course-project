using UnityEngine;

public class Timer : MonoBehaviour {

    private float timeleft = 0;
    private bool isStarted = false;
    private bool isPaused = false;

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
        isStarted = true;
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

    public void stopTimer() {
        isStarted = false;
    }
    /*
     * gets time in seconds
     * returns a string
     * formatted in 0:00
     * */
    static public string secondsToMinutes(float time) {
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
}
