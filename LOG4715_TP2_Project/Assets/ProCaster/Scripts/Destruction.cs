using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// cf : https://www.youtube.com/watch?v=BhwtIs5J8Ns	
public class Destruction : MonoBehaviour {

    public GameObject destructionPiece;

    [SerializeField]
    private float forceEffectiveDistance = 4f;

    [SerializeField]
    private float maxForce = 200;
	
	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bolt")
        {
			GameObject o = Instantiate(destructionPiece, transform.position, Quaternion.identity);
			Destroy(gameObject);


			foreach(Transform child in o.transform)
			{
                child.gameObject.AddComponent<Disappear>();
                Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), child.GetComponent<Collider>());

                Vector3 direction = child.transform.position - other.gameObject.transform.position;
                direction.x = 0;
                float magnitude = maxForce * (1 - direction.magnitude / forceEffectiveDistance);
                Vector3 force =  magnitude * direction.normalized;
				child.GetComponent<Rigidbody>().AddForce(force);
			}
        }
    }
}
