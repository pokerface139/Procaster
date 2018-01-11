using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

    public LevelManager levelManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelManager.ToggleVictoryMenu();
            Destroy(this.gameObject);
        }
    }
}
