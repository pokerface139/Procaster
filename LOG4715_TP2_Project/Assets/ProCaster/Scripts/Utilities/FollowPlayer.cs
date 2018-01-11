using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    [SerializeField]
    Transform Player;
    private Vector3 Offset;

    private void Start()
    {
        Offset = transform.position - Player.position;
        Offset.z = 0;
    }

    // Update is called once per frame
    void Update () {
        transform.position = Player.position + Offset;
	}
}
