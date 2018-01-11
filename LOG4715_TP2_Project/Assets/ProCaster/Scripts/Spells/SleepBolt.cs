using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBolt : BaseBolt {

    public float LifeTime { get; set; } = 4.0f;
    public float Damage { get; set; } = 15.0f;


    public GameObject Spark;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    public override bool FromPlayer() {
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ennemy")
        {
            other.gameObject.GetComponent<HealthBar>().TakeDamage(Damage);
        }

        LayerMask transparent = LayerMask.GetMask("Transparent");
        if (other.isTrigger == false && (1 << other.gameObject.layer) != transparent)
        {
            this.Destroy();
        }
    }

    public override void Destroy() {
        // Show spark
        GameObject spark = Instantiate(Spark);
        spark.transform.position = transform.position;
        spark.transform.forward = -transform.GetComponent<Rigidbody>().velocity;

        Destroy(spark, 2);
        Destroy(gameObject);
    }
}
