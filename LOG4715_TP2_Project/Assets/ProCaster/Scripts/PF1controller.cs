using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF1controller : MonoBehaviour
{
	public GameObject pf;
	public GameObject left;
	public GameObject right;
	
	public bool startGoingLeft;
    public float timeToCompleteCycle = 5f;
	private float speed;
	
	private float leftPosition;
	private float rightPosition;
	
	private bool pfIsGoingLeft;
	private bool pfIsGoingRight;
	
	private Transform playerTransform;
    private BoxCollider collider;
	private bool touchingPF;
	
	private PlayerControler pc;

	void Start ()
	{

		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        collider = pf.GetComponent<BoxCollider>();
		
		pc = playerTransform.GetComponent<PlayerControler>();

        left.SetActive(false);
        right.SetActive(false);

        leftPosition = left.transform.position.z;
		rightPosition = right.transform.position.z;

        float distance = 2 * Mathf.Abs(leftPosition - rightPosition);
        speed = distance / timeToCompleteCycle;


        if (startGoingLeft)
		{
			pfIsGoingLeft = true;
			pfIsGoingRight = false;
		}
		else
		{
			pfIsGoingLeft = false;
			pfIsGoingRight = true;
		}
		
		touchingPF = false;
	}
	
	void Update ()
	{
		
		if( (pf.transform.position.z - left.transform.position.z) <= 0.1 && (pf.transform.position.z - left.transform.position.z) >= -0.1)
		{
			pfIsGoingLeft = false;
			pfIsGoingRight = true;
		}
		if ( (pf.transform.position.z - right.transform.position.z) <= 0.1 && (pf.transform.position.z - right.transform.position.z) >= -0.1 )
		{
			pfIsGoingLeft = true;
			pfIsGoingRight = false;
		}
		
		if(pfIsGoingLeft)
		{
			pf.transform.Translate(Vector3.left * Time.deltaTime *speed);
			
			if(isNear())
			{
				if(pc._Flipped)
				{
					playerTransform.Translate(Vector3.forward * Time.deltaTime *speed);
				}
				else
				{
					playerTransform.Translate(Vector3.back * Time.deltaTime *speed);
				}
			}
			
			//if(touchingPF)
			//{
			//	playerTransform.Translate(Vector3.forward * Time.deltaTime *speed);
			//}
		}
		else
		{
			pf.transform.Translate(Vector3.right * Time.deltaTime *speed);
			
			if(isNear())
			{
				if(pc._Flipped)
				{
					playerTransform.Translate(Vector3.back * Time.deltaTime *speed);
				}
				else
				{
					playerTransform.Translate(Vector3.forward * Time.deltaTime *speed);
					
				}
			}
			
			//if(touchingPF)
			//{
			//	playerTransform.Translate(Vector3.back * Time.deltaTime *speed);
			//}
		}
	}
	
	private bool isNear()
	{
        //float distanceX = Mathf.Abs(playerTransform.position.x - pf.transform.position.x);
        //float distanceY = Mathf.Abs(playerTransform.position.y - pf.transform.position.y);
        //float distanceZ = Mathf.Abs(playerTransform.position.z - pf.transform.position.z);

        //if(distanceX < 0.5 && distanceY < 1.5 && distanceZ < 1.5 && playerTransform.position.y > pf.transform.position.y)
        //{
        //	return true;
        //}
        //else
        //{
        //	return false;
        //}

        Vector3 dims = collider.bounds.size / 2.0f;
        Vector3 center = pf.transform.position + dims.y * Vector3.up;
        LayerMask mask = LayerMask.GetMask("Floor");

        Collider[] colliders = Physics.OverlapBox(center, dims, Quaternion.identity);

        if (colliders.Length > 0)
        {
            foreach (Collider c in colliders)
            {
                if (c.gameObject.tag == "Player")
                {
                    return true;
                }
            }
        }

        return false;
    }

    void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "player")
		{
			touchingPF = true;
		}
	}
 
	void OnCollisionExit(Collision other)
	{
		if(other.gameObject.tag == "player")
		{
			touchingPF = false;
		}
	}    
}
