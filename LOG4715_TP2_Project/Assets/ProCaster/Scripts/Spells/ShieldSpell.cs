using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Bolt") {
            BaseBolt bolt = other.gameObject.GetComponent<BaseBolt>();
            if(!bolt.FromPlayer()) {
                bolt.Destroy();
            }
        }
    }

}
