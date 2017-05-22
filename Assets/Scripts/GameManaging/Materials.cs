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
        if (material == null) throw new System.Exception("Material passed is null.");
        if (objectToSet == null) throw new System.Exception("No GameObject supplied or GameObject is null. Materials.set()" + material);
        //set the new material
        objectToSet.GetComponent<Renderer>().material = material;
    }
}