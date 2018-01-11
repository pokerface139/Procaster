using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationEffect : MonoBehaviour {

    public float CycleTime = 2f;
    public float Amplitude = 0.2f;

    private Vector3 defaultPosition;
    private float timer = 0;

	// Use this for initialization
	void Start () {
        defaultPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= CycleTime)
        {
            timer -= CycleTime;
        }

        Vector3 position = defaultPosition + Amplitude * Mathf.Sin(2 * Mathf.PI / CycleTime * timer) * Vector3.up;
        transform.position = position;
	}
}
