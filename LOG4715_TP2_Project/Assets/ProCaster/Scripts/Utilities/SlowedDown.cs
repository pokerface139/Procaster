using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowedDown : MonoBehaviour {

    public float SpeedMultiplier = 0.5f;
    public float Duration = 1.5f;
    public PlayerControler player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        Destroy(this, Duration);
        if (player != null)
        {
            player.MoveSpeed *= SpeedMultiplier;
        }
	}
	

    private void OnDestroy()
    {
        if (player != null)
        {
            player.MoveSpeed /= SpeedMultiplier;
        }
    }


}
