using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    float xVelocity = 0;
    float yVelocity = 0;
    float mass = 80;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {        

        if (Input.GetKey(GameManager.GM.left))
        {
            // update with physics
        }
        if (Input.GetKey(GameManager.GM.right))
        {
            // update with physics
        }
        if (Input.GetKey(GameManager.GM.jump))
        {
            transform.position += Vector3.up / 2;
        }

        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);
    }
}
