using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour {

    public Material[] materials;
    static public Material[] staticMaterials;

    //copy the public array to the statuc public one
    private void Start() {
        staticMaterials = materials;
    }


    /*
     * set Material to a game Object
     *
     * argument 1 -> game object which the new material will be applied to
     * argument 2 -> the matieral name
     * */
    static public void set(GameObject objectToSet, string materialName) {
        Material newMat = null;

        //find the material by name in the array
        foreach (Material mat in staticMaterials) {
            if (mat.name == materialName) {
                newMat = mat;
            }
        }
        
        //if we couldn't find anything throw an exception
        if (newMat == null) throw new System.Exception("Couldn't find material: " + materialName + "Are you sure you have added it to MaterialsControl script?");

        //set the new material
        objectToSet.GetComponent<Renderer>().material = newMat;
    }
}