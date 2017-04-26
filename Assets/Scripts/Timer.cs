using UnityEngine;

public class Timer : MonoBehaviour {

    private float timeleft = 0;
    private bool start = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (start) runTimer();
	}

    bool runTimer() {
        if (timeleft > 0) {
            timeleft -= Time.deltaTime;
            return true;
        }
        start = false;
        return false;
    }

    public void startTimer(float timeleft) {
        if (!start) {
            this.timeleft = timeleft;
            start = true;
        }
    }

   public  float TimeRemaining() {
        return timeleft;
    }
}
