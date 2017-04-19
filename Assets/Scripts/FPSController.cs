using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour {

    static public int fps = 30;
    private void Start () {
        setFps(30);
    }
    static public void setFps(int newFps) {
        if (newFps != fps) {
            fps = newFps;
            Application.targetFrameRate = fps;
        }
    }
}
