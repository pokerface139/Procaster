using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodDoorController : MonoBehaviour {

    // Variables exposées
    [SerializeField]
    public int KeyId = 0;

    [SerializeField]
    private float AreaRadius = 1f;

    // Propriétés
    private Animator anim;
    private Transform player;
    private PlayerManager inventory;
    private bool isOpen = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventory = player.GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {

        if (isOpen)
        {
            return;
        }

        Vector3 distance = player.position - this.transform.position;
        distance.y = 0;

        if (distance.magnitude <= AreaRadius)
        {
            bool hasKey = inventory.Keys.Contains(KeyId);

            if (hasKey)
            {
                Debug.Log("Open!");
                isOpen = true;
                Physics.IgnoreCollision(player.GetComponent<Collider>(), this.GetComponent<Collider>());
                anim.SetBool("Open",isOpen);
            }
        }
	}
}
