using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    float xVelocity = 0;
    float yVelocity = 0;
    float xMax = .3f;
    float yMax = .3f;
    float mass = 80;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool hitKey = false;

        if (Input.GetKey(GameManager.GM.left))
        {
            if (xVelocity > -xMax)
            {
                xVelocity = xVelocity - .01f;
                hitKey = true;
            }        
        }
        if (Input.GetKey(GameManager.GM.right))
        {
            if (xVelocity < xMax)
            {
                xVelocity = xVelocity + .01f;
                hitKey = true;
            }       
        }
        if (Input.GetKey(GameManager.GM.jump))
        {
            transform.position += Vector3.up / 2;
            hitKey = true;
        }

        else if (!hitKey)
        {
            xVelocity = Physics.PHYS.calcFriction(xVelocity);
        }

        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);
    }
}
