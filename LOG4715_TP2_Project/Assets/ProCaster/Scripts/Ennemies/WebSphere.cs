using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSphere : MonoBehaviour {

    [SerializeField]
    float LifeTime = 6.0f;

    [SerializeField]
    public float Damage = 10.0f;

    [SerializeField]
    float SlowMultiplier = 0.5f;

    [SerializeField]
    float SlowDuration = 2f;

    void Start()
    {
        Destroy(this.gameObject, LifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthBar>().TakeDamage(Damage);
            SlowedDown slowEffect = other.gameObject.AddComponent<SlowedDown>();
            slowEffect.SpeedMultiplier = SlowMultiplier;
            slowEffect.Duration = SlowDuration;
        }

        if (other.gameObject.tag != "Ennemy") 
        {
            Destroy(this.gameObject);
        }
    }
}
