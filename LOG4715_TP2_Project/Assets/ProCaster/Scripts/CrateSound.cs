using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSound : MonoBehaviour
{

    public float PitchVariance = 0.1f;
    private new AudioSource audio;
    private Rigidbody rb;
    private float defaultPitch;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        defaultPitch = audio.pitch;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float pitch = Random.Range(defaultPitch - PitchVariance, defaultPitch + PitchVariance);
        audio.pitch = pitch;

        audio.volume = Mathf.Max(rb.velocity.magnitude/4f, 1);
        audio.Play();
    }
}
