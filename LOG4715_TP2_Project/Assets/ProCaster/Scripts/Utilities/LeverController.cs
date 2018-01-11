using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour {

    // Variables exposées
    [SerializeField]
    public DoorController Door;

    [SerializeField]
    public float distFromLeverToOpen = 1.5f;

    // Propriétés
    private Transform player;
    private Animator anim;
    private bool Activate { get; set; }


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Event
        Door.CloseEvent += Close;
    }

    void Update()
    {
        bool keyPressed = Input.GetKeyDown(KeyCode.E);
        Vector3 distance = transform.position - player.position;

        bool triggerdistance = distance.magnitude < distFromLeverToOpen;

        if (keyPressed && triggerdistance)
        {
            if (!Activate)
            {
                Open();
            }
            else
            {
                Close();
            }   
        }
    }

    public void Open()
    {
        Activate = true;
        anim.SetBool("Activated", Activate);
        Door.Open();
    }

    public void Close()
    {
        Activate = false;
        anim.SetBool("Activated", Activate);
        Door.Close();
    }
}
