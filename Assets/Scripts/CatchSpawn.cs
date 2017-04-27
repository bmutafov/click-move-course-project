using UnityEngine;

public class CatchSpawn : MonoBehaviour {

    public GameObject catchObject;
    public Material spawnCubeMaterial;
    public Material cubeTargetMat;

    static public string catchCubeName;

    // Use this for initialization
    void Start() {
        spawn();
    }

    // Update is called once per frame
    void Update() {
        if (!checkAliveCatch() && !GameManager.isGameOver) spawn();
    }

    /*  Spawns a new catch on a random position
     * 
     * */
   void spawn() {
        float randomX = Random.Range(Grid.start.x, Grid.size.x);
        

        //finding random player
        GameObject player = GameObject.Find("Player" + Random.Range(1, 3));
        player.GetComponent<Renderer>().material = cubeTargetMat;
        player.name = player.name + "Target";
        ParticleSystem ps = player.GetComponentInChildren<ParticleSystem>();
        if (!ps.isPlaying) {
            ps.Play();
        }

        //set the spawn position
        Vector3 spawnPosition = new Vector3(0.1f + randomX, 0.5f, 3.9f);

        //instantiate the catch on a position
        GameObject instObj = Instantiate(catchObject, spawnPosition, Quaternion.identity);
        instObj.name = "Catch";

        //tag and tell other scripts which is the new Catch position
        catchCubeName = "" + Mathf.Round(spawnPosition.z) + Mathf.Round(spawnPosition.x);
        GameObject cubeUnder = GameObject.Find(catchCubeName);
        cubeUnder.transform.tag = "CatchCubeSelected";
    }

    /*  CheckAliveCatch
     *  
     *  checks for alive catch
     *  
     *  returns true if theres an instance of catch alive
     *  
     *  returns false if there is no catch available
     * 
     * */
    bool checkAliveCatch() {
        GameObject alive = GameObject.Find("Catch");
        if (alive == null) return false;
        return true;
    }
}
    