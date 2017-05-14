 using UnityEngine;
using UnityEngine.UI;

public class CatchSpawn : MonoBehaviour {

    public GameObject catchObject;
    public Material spawnCubeMaterial;
    public Material cubeTargetMat;
    public Material grassTopSelected;

    static public string catchCubeName;
    static public GameObject lastPlayer;

    // Use this for initialization
    void Start() {
        //reference to the Timer script
        spawn();
    }

    // Update is called once per frame
    void Update() {
        if (!checkAliveCatch() && !GameManager.isGameOver) {
            spawn();
        }
    }

    /*  Spawns a new catch on a random position
     */
   void spawn() {
        //randomizing x position for catch object
        int randomX = Mathf.RoundToInt(Random.Range(Grid.start.x, (Grid.size.x - 1)));

        //selecting a new player who has to catch
        //cant be the previous one
        string newPlayer = selectNewPlayer();
        GameObject player = GameObject.Find(newPlayer);
        lastPlayer = player;

        //make the selected player a different color
        Materials.set(player, cubeTargetMat);

        //tag him so we know whos the player
        player.name = player.name + "Target";
        Materials.set(player.transform.parent.transform.FindChild("GrassTop").gameObject, grassTopSelected);

        //play a little particle system (explosion) on the new selected player
        ParticleSystem ps = player.GetComponentInChildren<ParticleSystem>();
        if (!ps.isPlaying) {
            ps.Play();
        }

        //set the spawn position
        GameObject spawnGridCube = GameObject.Find("" + (Grid.size.z - 1) + randomX);

        //get the dashed border and make it active
        Transform dashedBorder = spawnGridCube.transform.GetChild(0);
        dashedBorder.gameObject.SetActive(true);

        //tag the new cube so player can click on it
        spawnGridCube.transform.tag = "CatchCubeSelected";

        //instantiate the catch on a position
        GameObject instObj = Instantiate(catchObject, spawnGridCube.transform);
        instObj.name = "Catch";

        //set the new position to be centered on the grid
        instObj.transform.localPosition = new Vector3(0f, 3.5f, 0f);

        //tag and tell other scripts which is the new Catch position
        catchCubeName = spawnGridCube.transform.name;
    }

    /*
     * selects a new player to be the Active one
     * (who has to catch)
     * can't be the same as the previous one
     * returns string name of the new player
     * */
    string selectNewPlayer() {
        string newPlayer;
        //if it isnt first player
        if (lastPlayer != null) {
            //generate new player if is same as the previous one
            do {
                newPlayer = "Player" + Random.Range(1, Players.count + 1);
            } while (newPlayer == lastPlayer.transform.name);
        } else {
            //if first player just select the first one on random
            newPlayer = "Player" + Random.Range(1, Players.count + 1);
        }
        //return the name of the new player
        return newPlayer;
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
    