using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    // Pointeur de fonction
    public delegate void Action();
  
    // Variables exposées
    [SerializeField]
    private bool IsOpen = false;

    [SerializeField]
    private float OpeningSpeed = 3.0f;

    [SerializeField]
    private float ClosingSpeed = 1.0f;

    [SerializeField]
    private float ClosingDelay = 1.0f;

    // Properties
    private Action action;
    private float doorHeight;
    private Vector3 maxPos;
    private Vector3 minPos;
    private Rigidbody rb;
    private float timer = 0f;
    private float gain = 5f;


    public event Action CloseEvent;

	// Use this for initialization
	void Start () {
        if (IsOpen)
        {
            Open();
        }
        else
        {
            Close();
        }

        doorHeight = GetComponent<MeshRenderer>().bounds.size.y;
        minPos = transform.position;
        maxPos = minPos + Vector3.up * doorHeight * 3.0f / 4.0f;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        action();
    }

    public void Open()
    {
        timer = 0;
        IsOpen = true;
        action = OpeningAction;
    }

    public void Close()
    {
        timer = 0;
        IsOpen = false;
        action = ClosingAction;
    }

    private void OpeningAction()
    {
        if (transform.position.y <= maxPos.y)
        {
            MoveDoor(OpeningSpeed, Vector3.up);
        }
        else
        {
            rb.velocity = Vector3.zero;
            timer += Time.deltaTime;
            if (timer > ClosingDelay)
            {
                Close();
            }
        }
    }

    private void ClosingAction()
    {
        if (transform.position.y > minPos.y)
        {
            MoveDoor(ClosingSpeed, Vector3.down);
        }
        else
        {
            rb.velocity = Vector3.zero;

			CloseEvent();
			
            
        }
    }

    private void MoveDoor(float speed, Vector3 direction)
    {
        Vector3 error = speed * direction.normalized - rb.velocity;
		Vector3 force = gain * rb.mass * error;
        rb.AddForce(force);
    }
}
