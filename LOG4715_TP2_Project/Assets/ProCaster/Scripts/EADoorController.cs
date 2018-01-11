using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EADoorController : MonoBehaviour {

    [SerializeField]
    private uint Coins = 4;

    [SerializeField]
    private float AreaRadius = 2f;

    public GameObject Bubble;

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

        Vector3 distance = player.position - transform.position;

        if (distance.magnitude <= AreaRadius)
        {
            bool enoughCoins = inventory.Coins >= Coins;

            if (enoughCoins)
            {
                inventory.RemoveCoins(Coins);
                isOpen = true;
                Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
                anim.SetBool("Open", isOpen);

                if (Bubble != null)
                {
                    Bubble.SetActive(false);
                }
                
            }
        }
    }
}
