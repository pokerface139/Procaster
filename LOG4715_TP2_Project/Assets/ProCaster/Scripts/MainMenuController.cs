using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public string SceneName = "Game";

	public void LoadLevel()
	{
        PlayerPrefs.SetInt("RestartLastCheckpoint", 0);
        PlayerPrefs.Save();
        Time.timeScale = 1;
        SceneManager.LoadScene (SceneName);
        
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("RestartLastCheckpoint", 0);
        PlayerPrefs.Save();
        Application.Quit();
    }
		
}
