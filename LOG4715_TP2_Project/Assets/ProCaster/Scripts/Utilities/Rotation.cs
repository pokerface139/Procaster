using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    [SerializeField]
    float Period = 2.0f;

    [SerializeField]
    Vector3 Axis = new Vector3(0, 1, 0);

	// Update is called once per frame
	void Update () {
        transform.Rotate(360 * Time.deltaTime / Period * Axis);
	}
}
