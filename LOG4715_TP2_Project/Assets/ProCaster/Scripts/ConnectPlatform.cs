using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPlatform : MonoBehaviour {

	public ConnectPlatform Other;
	public bool Up = false;
	public Transform Top;
	public Transform Bottom;

	private Rigidbody rb;
	private BoxCollider collider;
	public List<GameObject> Objects;
	private float Speed = 2.0f;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody> ();
		collider = GetComponent<BoxCollider> ();
		Objects = new List<GameObject> ();

		if (Up) {
			transform.position = Top.transform.position;
		} else {
			transform.position = Bottom.transform.position;
		}

		Top.gameObject.SetActive (false);
		Bottom.gameObject.SetActive (false);
	}


	// Update is called once per frame
	void Update () {
		ObjectsAbove ();

		if (transform.position.y > Top.position.y) {
			rb.velocity = Vector3.zero;
			transform.position = Top.position;

		} else if (transform.position.y < Bottom.position.y) {
			rb.velocity = Vector3.zero;
			transform.position = Bottom.position;

		} else {

			rb.isKinematic = false;

			if (TotalMass() - Other.TotalMass()  > 0 && transform.position.y != Bottom.position.y) {
				
				rb.velocity = Vector3.down * Speed;
			} else if (TotalMass() - Other.TotalMass() < 0 && transform.position.y != Top.position.y) {
				rb.velocity = Vector3.up * Speed;
			}
			else {
				rb.velocity = Vector3.zero;
				rb.isKinematic = true;
			}
		}
	}

	void ObjectsAbove()
	{
		Vector3 dims = collider.bounds.size / 2.0f;
		Vector3 center = transform.position + dims.y * Vector3.up;
		LayerMask mask = LayerMask.GetMask ("Floor");

		Collider[] colliders = Physics.OverlapBox (center, dims, Quaternion.identity);
		Objects.Clear ();
		if (colliders.Length > 0) {
			foreach (Collider c in colliders) {
				if (c.gameObject.GetComponent<Rigidbody> () != null) {
					Objects.Add (c.gameObject);
				}
			}
		}
	}

	/*
	void OnCollisionEnter(Collision other)
	{
		if (!Objects.Contains (other.gameObject)) {
			if (other.transform.position.y > transform.position.y && other.transform.GetComponent<Rigidbody>() != null) {
				Objects.Add (other.gameObject);
				Debug.Log (other.gameObject.name);
			} 
		} 
	}

	void OnCollisionExit(Collision other)
	{
		if (Objects.Contains (other.gameObject)) {
			Objects.Remove (other.gameObject);
			Debug.Log (other.gameObject.name);
		}
	}
	*/

	public float TotalMass()
	{
		float mass = 0;
		foreach (GameObject o in Objects) {
			mass += o.GetComponent<Rigidbody> ().mass;
		}
		return mass;
	}
}
