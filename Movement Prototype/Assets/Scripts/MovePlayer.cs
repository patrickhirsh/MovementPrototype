using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Player attributes
    float xVelocity = 0;
    float yVelocity = 0;
    float mass = 80;
    float xMax = .3f;
    float yMax = .3f;

    // Collision identifiers - identifies when key strokes should be accepted
    bool rightValid = true;
    bool leftValid = true;
    bool jumpValid = true;

    void Start ()
    {

    }
	

    // Update is called once per frame
    void Update()
    {

        // Respond to key events
        if (Input.GetKey(GameManager.GM.left) && leftValid)
        {
            if (xVelocity > -xMax)
            {
                xVelocity = xVelocity - .01f;
            }
        }
        if (Input.GetKey(GameManager.GM.right) && rightValid)
        {
            if (xVelocity < xMax)
            {
                xVelocity = xVelocity + .01f;
            }
        }
        if (Input.GetKey(GameManager.GM.jump) && jumpValid)
        {
            transform.position += Vector3.up / 2;
        }


        List<GameObject> collisions = Collision.COL.checkCollision(this.gameObject);


        // If there was a wall collision
        if (collisions[0] != null)
        {
            xVelocity = 0;
        }

        // If there was a floor/ceiling collision
        if (collisions[1] != null)
        {
            yVelocity = 0;
            jumpValid = true; // this needs to change
            xVelocity = Physics.PHYS.calcFriction(xVelocity);
        }
        else
            yVelocity = Physics.PHYS.calcGravity(yVelocity); // this will get you stuck on the ceiling

        // Apply velocity change
        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);
    }
}
