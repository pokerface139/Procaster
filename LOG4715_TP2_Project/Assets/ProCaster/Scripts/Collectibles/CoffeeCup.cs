using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCup : MonoBehaviour {

    // Propriétés exposées
    [SerializeField]
    float HealthPoint = 25.0f;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.gameObject.tag == "Player")
        {
            activated = true;

            other.gameObject.GetComponent<HealthBar>().Heal(HealthPoint);

            GetComponent<AudioSource>().Play();

            Destroy(this.gameObject, 0.15f);
        }
    }
}
