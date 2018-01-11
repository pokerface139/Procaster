using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAt : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
        transform.LookAt(target);
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);
    }
}
