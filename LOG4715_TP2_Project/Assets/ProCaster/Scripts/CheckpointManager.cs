using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour {

	public Canvas canvas;
	public float TimeCheckpointDisplay = 2f;

	private List<Checkpoint> checkpoints;
	private int currentIndex = -1;
	private Transform player;
	private float timer = 0f;


	// Use this for initialization
	void Start () {
		checkpoints = new List<Checkpoint> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		foreach (Transform t in transform)
		{
			Checkpoint cp = t.GetComponent<Checkpoint> ();
			if (cp != null) {
				cp.OnTrigger += this.OnCheckpointTriggered;
				checkpoints.Add (cp);
			}
		}
			
		if (PlayerPrefs.HasKey ("Checkpoint") && PlayerPrefs.HasKey ("RestartLastCheckpoint")) {
			if (PlayerPrefs.GetInt ("RestartLastCheckpoint") == 1) {
				currentIndex = PlayerPrefs.GetInt ("Checkpoint");
				PlayerPrefs.SetInt ("RestartLastCheckpoint", 0);
				PlayerPrefs.Save ();
			}	
		}

		if (currentIndex >= 0) {
            Vector3 position = checkpoints[currentIndex].GetComponent<Transform>().position;
            Vector3 distance = position - transform.position;
            distance.x = 0;
            Camera.main.transform.position += distance;

            player.position = position;

			for (int i = 0; i < currentIndex; ++i) {
				checkpoints [i].Triggered = true;
			}
		}
	}

	void Update()
	{
		if (canvas != null && canvas.gameObject.activeSelf) {
			timer += Time.deltaTime;
			if (timer >= TimeCheckpointDisplay) {
				canvas.gameObject.SetActive (false);
				timer = 0;
			}
		}	
	}

	void OnCheckpointTriggered(Checkpoint cp)
	{
		currentIndex = checkpoints.IndexOf(cp);
        GetComponent<AudioSource>().Play();
		if (canvas != null && !canvas.gameObject.activeSelf) {
			canvas.gameObject.SetActive (true);
		}
	}

	void OnDestroy()
	{
		PlayerPrefs.SetInt ("Checkpoint", currentIndex);
		PlayerPrefs.Save ();
	}

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("RestartLastCheckpoint", 0);
        PlayerPrefs.Save();
    }
}
