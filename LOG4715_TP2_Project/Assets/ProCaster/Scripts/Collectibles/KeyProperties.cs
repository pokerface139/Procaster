using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyProperties : MonoBehaviour {

    [SerializeField]
    public int KeyId = 0;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.gameObject.tag == "Player")
        {
            activated = true;

            PlayerManager inventory = other.GetComponent<PlayerManager>();
            inventory.AddKey(KeyId);

            GetComponent<AudioSource>().Play();

            Destroy(this.gameObject, 0.15f);
        }
    }
}
