using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public delegate void TriggerDelegate(Checkpoint cp);
	public event TriggerDelegate OnTrigger;
	public bool Triggered = false;

	void OnTriggerEnter(Collider other)
	{
		if (!Triggered && other.gameObject.tag == "Player") {
			Triggered = true;
			if (OnTrigger != null) {
				OnTrigger (this);
			}
		}
	}
}
