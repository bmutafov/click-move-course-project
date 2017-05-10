using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MoveDetect : MonoBehaviour {

    public GameObject[] objectsToDeactivate;

    private IEnumerator OnTriggerEnter (Collider other) {
        foreach(GameObject obj in objectsToDeactivate) {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("scene01");
    }
}
