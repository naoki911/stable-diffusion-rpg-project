using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private GameObject cloneObj;
    public Material coverMaterial;

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.tag == "Player"){
            Debug.Log("enter");
            cloneObj = Instantiate(this.gameObject,gameObject.transform.position,gameObject.transform.rotation,gameObject.transform.parent);
            Destroy(cloneObj.GetComponent<ItemObject>());
            cloneObj.GetComponent<Renderer>().material = coverMaterial;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player"){
            Destroy(cloneObj);
        }
    }
}
