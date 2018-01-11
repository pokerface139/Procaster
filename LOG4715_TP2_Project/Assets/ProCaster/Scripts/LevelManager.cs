using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	private HealthBar playerHealth;
	public GameObject RestartMenu;
	public GameObject EscMenu;
    public GameObject VictoryMenu;
    public float restartDelay = 1f;
	private float timer = 0f;


	// Use this for initialization
	void Start () {
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<HealthBar> ();
		RestartMenu.SetActive (false);
		//RestartMenu.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener(this.Restart);
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsPlayerDead ()) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				if (EscMenu != null) {

					EscMenu.SetActive (!EscMenu.activeSelf);
					if (EscMenu.activeSelf) {
						Time.timeScale = 0;
					} else {
						Time.timeScale = 1;
					}
				}
			}
		}
	}

    public void ToggleVictoryMenu()
    {
        VictoryMenu.SetActive(!VictoryMenu.activeSelf);
        if (VictoryMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

	private bool IsPlayerDead()
	{
		if (playerHealth.Dead) { 
			if (timer < restartDelay) {
				timer += Time.deltaTime;
			} else if (!RestartMenu.activeSelf) {
				RestartMenu.SetActive (true);
			}
		}

		return playerHealth.Dead;
	}

	public void Restart()
	{
		PlayerPrefs.SetInt("RestartLastCheckpoint",0);
		PlayerPrefs.Save ();
		Reload ();
	}

	public void RestartLastCheckpoint()
	{
		PlayerPrefs.SetInt("RestartLastCheckpoint",1);
		PlayerPrefs.Save ();
		Reload ();
	}

	public void MainMenu()
	{
		PlayerPrefs.SetInt("RestartLastCheckpoint",0);
		PlayerPrefs.Save ();
		SceneManager.LoadScene ("MainMenuScene");
	}

	private void Reload()
	{
		Time.timeScale = 1;
		Scene scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (scene.name);
	}

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
