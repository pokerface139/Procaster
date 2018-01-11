using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : MonoBehaviour {

    public Queue<Message> messages;
    private Message currentMessage;
    private CharacterDialog dialogBox;
    private float timer = 0f;

    private void Start()
    {
        messages = new Queue<Message>();
        dialogBox = GetComponent<CharacterDialog>();
        currentMessage = null;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            dialogBox.Displayed = messages.Count != 0;
            if (dialogBox.Displayed)
            {
                currentMessage = messages.Dequeue();
                timer = currentMessage.ShowTime;
                dialogBox.Text = currentMessage.Text;
            }
        }
        else {
            timer -= Time.deltaTime;
        }
    }

    public void AddMessage(Message message)
    {
        messages.Enqueue(message);
    }

}
