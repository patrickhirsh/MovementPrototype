using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    AABBCollidable player;

    // Player attributes
    float xVelocity = 0;
    float yVelocity = 0;
    float mass = 80;
    float acceleration = .02f;
    float sharpAcceleration = .05f;
    float slowAcceleration = .015f;
    float xMax = .15f;
    float yMax = .15f;

    // Determines if a jump should register
    bool jumpValid = true;

    void Start ()
    {
        player = new AABBCollidable(gameObject);
    }
	

    // Update is called once per frame
    void Update()
    {

        // Apply gravity
        yVelocity = Physics.PHYS.calcGravity(yVelocity);

        // Respond to "Left" key
        if (Input.GetKey(GameManager.GM.left))
            if (xVelocity > -xMax)
            {
                if (jumpValid)
                {
                    if (xVelocity > 0)
                        xVelocity -= sharpAcceleration;

                    else
                        xVelocity -= acceleration;
                }
                else
                    xVelocity -= slowAcceleration;
            }
                

        // Respond to "Right" key
        if (Input.GetKey(GameManager.GM.right))
            if (xVelocity < xMax)
            {
                if (jumpValid)
                {
                    if (xVelocity < 0)
                        xVelocity += sharpAcceleration;

                    else
                        xVelocity += acceleration;
                }
                else
                    xVelocity += slowAcceleration;         
            }
                

        // Respond to "Jump" key
        if (Input.GetKey(GameManager.GM.jump) && jumpValid)
            yVelocity += .4f;

        // Apply velocity change
        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);

        // Check for AABB gameObject collisions
        List<float> correction = Collision.COL.checkCollisionAABB(player);

        // Handle AABB gameObject collisions
        handleCollisionsAABB(correction);

        
    }


    void handleCollisionsAABB(List<float> correction)
    {
        jumpValid = false;

        // A horizontal collision occured
        if (correction[0] != 0)
        {
            // Velocity should only be set to zero if the correction and velocity directions are opposite
            if (((correction[0] > 0) && (xVelocity < 0)) || ((correction[0] < 0) && (xVelocity > 0)))
                xVelocity = 0;

            transform.position = new Vector3(transform.position.x + correction[0], transform.position.y, transform.position.z);
        }

        // A vertical collision occured
        if (correction[1] != 0)
        {
            yVelocity = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y + correction[1], transform.position.z);

            // The player is currectly on the ground
            if (correction[1] > 0)
            {
                if (!(Input.GetKey(GameManager.GM.right) || Input.GetKey(GameManager.GM.left)))
                {
                    xVelocity = Physics.PHYS.calcFriction(xVelocity);
                }
                    

                jumpValid = true;
            }           
        }
    }
}
