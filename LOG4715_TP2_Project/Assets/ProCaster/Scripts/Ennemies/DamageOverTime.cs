using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour {

    [SerializeField]
    private float Damage = 15f;

    [SerializeField]
    private float Period = 1f;

    [SerializeField]
    private bool DamageOnEnter = false;

    private float timer = 0f;

    private void Start()
    {
        if (DamageOnEnter)
        {
            timer = Period;
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        ApplyDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        ApplyDamage(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (DamageOnEnter)
        {
            timer = Period;
        }
    }

    private void ApplyDamage(Collider other)
    {
        if (other.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer >= Period)
            {
                other.gameObject.GetComponent<HealthBar>().TakeDamage(Damage);
                timer = 0;
            }
        }
    }
}
