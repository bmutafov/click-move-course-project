﻿using UnityEngine;

public class HighlightController : MonoBehaviour {

	[Header("Materials")]
	public Material normalMat;
	public Material selectedMat;
    public Material cubeCatchMat;
    public Material targetMat;
    public Material bonusMat;

    enum DeselectType {
        Normal, //a normal cube in the grid
        CatchGrid, //a grid cube in the catch row, but doesn't have the catch on it
        CatchSpawn, //a grid  cube in the catch row, with the catch
        MoveTarget, //the target where you have to move the cube with the catch
        BonusCube //bonus cube for different purposes
    };

    static public Vector3 catchDestination;

    /*
     * Deselects a grid cube with the given string name ( in format zx )
     * DeselectType tells what atributes to put on the new deselected cube
     * 
     * Normal -> available cube, tag: GridCube
     * CatchGrid -> unavailable cube, tag: CatchCube
     * CatchSpawn -> available only for certain player: Tag: CatchCubeSelected
     * MoveTarget -> available, target cube, tag: GridCube
     * */
    void deselect(string name, DeselectType type) {
        string newTag = null;
        Material newMat = null;
        GameObject cube = GameObject.Find(name);
        if (cube != null) {
            switch (type) {
                case DeselectType.Normal:
                newTag = "GridCube";
                newMat = normalMat;
                break;

                case DeselectType.CatchGrid:
                newTag = "CatchCube";
                newMat = cubeCatchMat;
                break;

                case DeselectType.CatchSpawn:
                newTag = "CatchCubeSelected";
                newMat = cubeCatchMat;
                break;

                case DeselectType.MoveTarget:
                newTag = "GridCube";
                newMat = targetMat;
                break;

                case DeselectType.BonusCube:
                newTag = "BonusCube";
                newMat = bonusMat;
                break;
            }
            cube.tag = newTag;
            Materials.set(cube, newMat);
        }
    }

    //Deselects all cubes on the field
    public void deselectAll() {
        //0->grid size X
        for (int x = 0; x < Grid.size.x; x++) {
            //0->grid size Z
            for (int z = 0; z < Grid.size.z; z++) {
                //Get the current cube name
                string currentGrid = "" + z + x;

                //if it is the grid cube with the catch
                if (z == Grid.size.z - 1 && CatchSpawn.catchCubeName == currentGrid) deselect(currentGrid, DeselectType.CatchSpawn);

                //if any other catch cube with no catch on it
                else if (z == Grid.size.z - 1) deselect(currentGrid, DeselectType.CatchGrid);

                //if the move target cube
                else if (x == catchDestination.x && z == catchDestination.z) deselect(currentGrid, DeselectType.MoveTarget);

                else if (Grid.isBonusDestination(x, z)) deselect(currentGrid, DeselectType.BonusCube);

                //if any othger normal grid cube
                else deselect(currentGrid, DeselectType.Normal);
            }
        }
    }

    public void findPath(Transform player) {
		selectRow (player.position);
		selectCol (player.position);
	}

	void selectRow(Vector3 pos) {
		int done = 1;
		bool xUpAllow = true, xDownAllow = true;
		GameObject nextCube, prevCube;

		//Cycling through the grid on X axis, starting from Players pos to left and right
		for (float i = pos.x + 1; done < Grid.size.x; done++, i++) {
			nextCube = GameObject.Find ("" + pos.z + i);
			prevCube = GameObject.Find ("" + pos.z + (i - 2*done));

			//if NextCube exists change its material and make it available
			xUpAllow = checkAndHighlight(nextCube, xUpAllow);

			//if PrevCube exists change its material and make it available
			xDownAllow = checkAndHighlight(prevCube, xDownAllow);
		}
	}

	void selectCol(Vector3 pos) {
		int done = 1;
		bool zUpAllow = true, zDownAllow = true;
		GameObject nextCube, prevCube;

		//Cycling through the grid on Z axis, starting from Players pos to left and right
		for (float i = pos.z + 1; done < Grid.size.z; done++, i++) {
			nextCube = GameObject.Find ("" + i + pos.x);
			prevCube = GameObject.Find ("" + (i - 2*done) + pos.x);

			//if NextCube exists change its material and make it available
			zUpAllow = checkAndHighlight(nextCube, zUpAllow);

			//if PrevCube exists change its material and make it available
			zDownAllow = checkAndHighlight(prevCube, zDownAllow);
		}
	}
		
	//Checks if theres an object above the given
	//returns true if no
	//returns false if yes
	static public bool isFree(GameObject nextCube) {
        if (nextCube == null) return false;
        
		RaycastHit hit;
        if (Physics.Raycast(nextCube.transform.position, Vector3.up, out hit)) {
            //if theres object different than catch, the space is taken
            if (hit.transform.tag != "Catch") {
                return false;
            }
		}
		return true;
	}

    //Highlights cube if available
    //Returns true if highlighted
    //Returns false if not ( occupied )
    bool checkAndHighlight(GameObject cube, bool allow) {
        if (cube != null) {
            if (isFree(cube) && allow) {
                highlightCube(cube);
                return true;
            }
        }
        return false;
    }

    /*
     * Highlights the cube
     * */
    void highlightCube(GameObject cube) {
        bool available = false;
        //Ignore CatchCubesMoveController.activePlayer.name.Contains("Target")
        //Doesnt ignore CatchCubeSelected
        if (cube.name == CatchSpawn.catchCubeName && MoveController.activePlayer.name.Contains("Target")) {
            available = true;
        }
        else if (cube.tag != "CatchCube" && cube.tag != "CatchCubeSelected") {
            available = true;
        }
        if (available) {
            Materials.set(cube, selectedMat);
            cube.tag = "Selected";
        }
    }

    /*
     * Deactivates all borders
     * */
    static public void deactivateAllDashedBorders() {
        GameObject[] dashedBorders = GameObject.FindGameObjectsWithTag("Dash");
        foreach(GameObject border in dashedBorders) {
            border.SetActive(false);
        }
    }
    /*
      Change tags Taken to tags CridCube 
      Taken is used to fix the bug in the randomFreePosition function in Players script
    */
    public void setDefaultTags() {
        GameObject[] gridCubes = GameObject.FindGameObjectsWithTag("Taken");
        for (int i = 0; i < gridCubes.Length; i++) {
            gridCubes[i].tag = "GridCube";
        }
    }

}