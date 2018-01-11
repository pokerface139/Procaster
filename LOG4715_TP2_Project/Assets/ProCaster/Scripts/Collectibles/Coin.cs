using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    private bool activated = false;

    void OnTriggerEnter(Collider collider)
    {
        if (!activated && collider.gameObject.tag == "Player")
        {
            activated = true;

            collider.gameObject.GetComponent<PlayerManager>().AddCoins(1);

            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }

            GetComponent<AudioSource>().Play();

            Destroy(this.gameObject,1);
        }
    }
}
