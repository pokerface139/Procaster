using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpController : MonoBehaviour {

    [SerializeField]
    float JumpHeight = 3f;

    [SerializeField]
    float JumpTime = 0.5f;

    [SerializeField]
    float Damage = 10f;

    [SerializeField]
    float TimeBetweenJump = 1f;

    [SerializeField]
    float TimeOffset = 0f;

    private Vector3 startPosition;
    private Vector3 startForward;
    private bool jumping = false;
    private float timer = 0f;
    private float jumpSpeed = 3f;
    private float acceleration = -9.8f;
    private float speed = 0f;

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
        startForward = transform.forward;
        timer = -TimeOffset;
	}
	
	// Update is called once per frame
	void Update () {

        acceleration = -2f * JumpHeight / Mathf.Pow(JumpTime / 2f, 2);
        jumpSpeed = -acceleration * JumpTime / 2f;

        if (jumping)
        {
            Move();

            if (transform.position.y <= startPosition.y)
            {

                speed = 0;
                transform.position = startPosition;
                transform.forward = startForward;
                jumping = false;
            }
            else
            {
                transform.forward = Vector3.up * speed;
            }

        }
        else
        {
            timer += Time.deltaTime;
            if (timer > TimeBetweenJump)
            {
                timer = 0;
                jumping = true;
                speed = jumpSpeed;
            }
        }
    }


    private void Move()
    {
        speed = speed + acceleration * Time.deltaTime;
        Vector3 deltaY = speed * Time.deltaTime * Vector3.up;
        transform.position = transform.position + deltaY;
    }
}
