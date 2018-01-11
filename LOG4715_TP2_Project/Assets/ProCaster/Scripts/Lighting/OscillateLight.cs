using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class OscillateLight : MonoBehaviour {

    [SerializeField]
    private float Variance = 0.2f;
    [SerializeField]
    private float MinimumFlickerDamping = 0.1f;
    [SerializeField]
    private float MaximumFlickerDamping = 0.2f;
    [SerializeField]
    private bool StopFlickering = false;

    private Light lighting;
    private float baseIntensity;
    private bool flickering = false;
 

	// Use this for initialization
	void Start () {
        lighting = GetComponent<Light>();
        baseIntensity = lighting.intensity;

        if (!StopFlickering && !flickering)
        {
            StartCoroutine(this.Flick());
        }    
	}
	
	// Update is called once per frame
	void Update () {
        if (!StopFlickering && !flickering)
        {
            StartCoroutine(this.Flick());
        }
    }

    private IEnumerator Flick()
    {
        flickering = true;
        while (!StopFlickering)
        {
            float intensity = Random.Range(baseIntensity - Variance, baseIntensity + Variance);
            lighting.intensity = intensity;
            yield return new WaitForSeconds(Random.Range(MinimumFlickerDamping, MaximumFlickerDamping));
        }
        flickering = false;
    }
}
