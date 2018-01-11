using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MessageTrigger : MonoBehaviour {

    public Message message = new Message();
    public bool once = false;
    public bool trigeredByEKey = false;

    private bool seen = false;
    private MessageCenter center;
    
    private bool closeEnough = false;
   
    // Use this for initialization
    void Start () {
        center = GameObject.FindObjectOfType<MessageCenter>();
	}

    private void OnTriggerStay(Collider other)
    {
        if (closeEnough && Input.GetKey(KeyCode.E))
        {
            if (!center.messages.Contains(message))
            {
                center.AddMessage(message);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (center != null && other.tag == "Player")
        {
            closeEnough = true;

            if (!trigeredByEKey)
            {
                if (seen && once)
                {
                    return;
                }

                center.AddMessage(message);
                seen = true;
            }
        } 
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (center != null && other.tag == "Player")
        {
            closeEnough = false;
        }
    }

}
