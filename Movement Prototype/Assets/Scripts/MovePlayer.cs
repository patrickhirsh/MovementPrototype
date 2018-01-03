using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    float xVelocity = 0;
    float yVelocity = 0;
    float xDelta = .01f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {        

        if (Input.GetKey(GameManager.GM.left))
        {
            xVelocity -= xDelta;
        }
        if (Input.GetKey(GameManager.GM.right))
        {
            xVelocity += xDelta;
        }
        if (Input.GetKey(GameManager.GM.jump))
        {
            transform.position += Vector3.up / 2;
        }

        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);
    }
}
