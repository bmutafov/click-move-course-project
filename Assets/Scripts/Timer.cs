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
}
