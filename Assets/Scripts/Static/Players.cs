using UnityEngine;

public class Players : MonoBehaviour {

    static public int count;

    private void Start () {
        count = transform.childCount;
    }

    static public Vector3 randomFreePosition () {
        int i = 0;
        GameObject[] gridCubes = GameObject.FindGameObjectsWithTag("GridCube");
        do {
            int randomPos = Random.Range(0, gridCubes.Length - 1);
            GameObject randomCube = gridCubes[randomPos];
            if (HighlightController.isFree(randomCube) && randomCube.transform.tag != "CatchCube" && randomCube.transform.tag != "Taken") {
                gridCubes[randomPos].tag = "Taken";
                return randomCube.transform.position;
            }
            i++;
        } while (i < gridCubes.Length);
        throw new System.Exception("No free cubes!");
    }

}
