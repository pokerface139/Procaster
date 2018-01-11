using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBoxesTrap : MonoBehaviour {
    public GameObject boxesTrap;
    public Light light;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (this.boxesTrap && other.transform.tag.Equals("Bolt") )
        {
            light.GetComponent<Light>().enabled = true;
            Destroy(this.boxesTrap);
        }
    }
}
