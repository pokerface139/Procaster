using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour {

    // Variables exposés
    [SerializeField]
    private float TimeBeforeDisappear = 1.0f;

    // Propriétés
    private float timer = 0;

    private Vector3 initialScale;

    private bool disappearing = false;

    // Use this for initialization
    void Start () {
        initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        // Update timer
        timer += Time.deltaTime;

        if (timer > TimeBeforeDisappear && !disappearing)
        {
            disappearing = true;
            StartCoroutine(Shrink());
        }
	}


    IEnumerator Shrink()
    {
        Vector3 currentScale = transform.localScale;
        Vector3 minimumScale = initialScale * 0.05f;

        while (currentScale.magnitude > minimumScale.magnitude)
        {
            transform.localScale = 0.95f * transform.localScale;
            currentScale = transform.localScale;

            yield return new WaitForSeconds(0.01f);
        }

        Destroy(this.gameObject);
    }
}
