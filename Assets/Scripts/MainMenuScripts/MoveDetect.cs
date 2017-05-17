using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MoveDetect : MonoBehaviour {

    public GameObject[] objectsToDeactivate;

    private GameManager gm;

    private void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter (Collider other) {
        foreach(GameObject obj in objectsToDeactivate) {
            obj.SetActive(false);
        }
        StartCoroutine(gm.sceneFade("scene01"));
    }
}
