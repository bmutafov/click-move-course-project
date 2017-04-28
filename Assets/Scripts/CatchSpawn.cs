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
        //roundomizing x position for catch object
        int randomX = Mathf.RoundToInt(Random.Range(Grid.start.x, (Grid.size.x - 1)));
  
        //finding random player
        //TODO change range
        GameObject player = GameObject.Find("Player" + Random.Range(1, 3));
        Materials.set(player, cubeTargetMat.name);
        player.name = player.name + "Target";
        ParticleSystem ps = player.GetComponentInChildren<ParticleSystem>();
        if (!ps.isPlaying) {
            ps.Play();
        }

        //set the spawn position
        GameObject spawnGridCube = GameObject.Find("" + Grid.size.z + randomX);
        spawnGridCube.transform.tag = "CatchCubeSelected";

        //instantiate the catch on a position
        GameObject instObj = Instantiate(catchObject, spawnGridCube.transform);
        instObj.name = "Catch";
        instObj.transform.localPosition = new Vector3(0f, 3f, 0f);

        //tag and tell other scripts which is the new Catch position
        catchCubeName = spawnGridCube.transform.name;
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
    