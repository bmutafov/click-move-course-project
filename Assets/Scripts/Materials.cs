using UnityEngine;

public class Materials : MonoBehaviour {
    /*
     * set Material to a game Object
     *
     * argument 1 -> game object which the new material will be applied to
     * argument 2 -> the matieral name
     * */
    static public void set(GameObject objectToSet, Material material) {
        //if we couldn't find anything throw an exception
        if (material == null) throw new System.Exception("Couldn't find material: " + material.name + "Are you sure you have added it to MaterialsControl script?");
        if (objectToSet == null) throw new System.Exception("No GameObject supplied or GameObject is null. Materials.set()");
        //set the new material
        objectToSet.GetComponent<Renderer>().material = material;
    }
}