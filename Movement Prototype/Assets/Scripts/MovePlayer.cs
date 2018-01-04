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

    // Determines if a jump should register
    bool jumpValid = true;

    // Lists of different collidable types
    List<GameObject> collisionsAABB;

    void Start ()
    {
        List<GameObject> collisionsAABB = new List<GameObject>();
    }
	

    // Update is called once per frame
    void Update()
    {

        // Apply gravity
        yVelocity = Physics.PHYS.calcGravity(yVelocity);

        // Respond to "Left" key
        if (Input.GetKey(GameManager.GM.left))
            if (xVelocity > -xMax)
                xVelocity = xVelocity - .01f;

        // Respond to "Right" key
        if (Input.GetKey(GameManager.GM.right))
            if (xVelocity < xMax)
                xVelocity = xVelocity + .01f;

        // Respond to "Jump" key
        if (Input.GetKey(GameManager.GM.jump) && jumpValid)
            yVelocity += .4f;

        // Check for AABB gameObject collisions
        List<GameObject> collisionsAABB = Collision.COL.checkCollisionAABB(this.gameObject);

        // Handle AABB gameObject collisions
        handleCollisionsAABB(collisionsAABB);

        // Apply velocity change
        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);
    }

    void handleCollisionsAABB(List<GameObject> collisionsAABB)
    {

        // If there was a wall collision
        if (collisionsAABB[0] != null)
        {
            xVelocity = 0;

            // Determine the correction amount
            float collisionDistance = Mathf.Abs(collisionsAABB[0].transform.position[0] - this.gameObject.transform.position[0]);
            float correctDistance = this.gameObject.GetComponent<Renderer>().bounds.size[0] / 2 + collisionsAABB[0].GetComponent<Renderer>().bounds.size[0] / 2;
            float correction = correctDistance - collisionDistance + .001f;

            // Wall is right of the player, correct left
            if (collisionsAABB[0].transform.position[0] > this.gameObject.transform.position[0])
                transform.position = new Vector3(transform.position.x - correction, transform.position.y, transform.position.z);

            // Wall is left of the player, correct right
            else
                transform.position = new Vector3(transform.position.x + correction, transform.position.y, transform.position.z);

        }

        // If there was a floor/ceiling collision
        if (collisionsAABB[1] != null)
        {
            yVelocity = 0;

            // Determine the correction amount
            float collisionDistance = Mathf.Abs(collisionsAABB[1].transform.position[1] - this.gameObject.transform.position[1]);
            float correctDistance = this.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 + collisionsAABB[1].GetComponent<Renderer>().bounds.size[1] / 2;
            float correction = correctDistance - collisionDistance + .001f;

            // Collision is with a ceiling, correct down, disable jumping
            if (collisionsAABB[1].transform.position[1] > this.gameObject.transform.position[1])
                transform.position = new Vector3(transform.position.x, transform.position.y - correction, transform.position.z);

            // Collision is with the ground, correct up, apply friction
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + correction, transform.position.z);
                xVelocity = Physics.PHYS.calcFriction(xVelocity);
                jumpValid = true;
            }
        }

        else
            jumpValid = false;
    }
}
