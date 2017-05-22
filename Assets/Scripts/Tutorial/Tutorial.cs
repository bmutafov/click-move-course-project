using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject[] tutorialBoxes;

    private int tutorials;
    private int current = 1;

    private void Awake () {
        tutorials = tutorialBoxes.Length;
    }
    
    public void changeTutorialBox() {
        if (current == tutorials) return;
        tutorialBoxes[current].SetActive(true);
        if (current > 0) tutorialBoxes[current - 1].SetActive(false);
        current++;
    }
}
