using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    AABBCollidable player;

    // Player attributes
    float xVelocity = 0;
    float yVelocity = 0;
    float acceleration = .022f;
    float accelerationPivot = .06f;
    float accelerationAirborne = .02f;
    float mass = 80;
    float xMax = .15f;
    float yMax = .15f;

    // Movement state identifiers
    bool airborne;
    bool frictionGround;
    bool frictionAir;
    bool frictionWall;

    void Start ()
    {
        player = new AABBCollidable(gameObject);
    }
	

    // Update is called once per frame
    void Update()
    {
        // Adjust player velocity based on forces due to physics
        applyPhysics();

        // Adjust player velocity based on keystrokes
        applyKeystrokes();

        // Apply velocity change
        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);

        // Movement identifiers for this cycle have been applied, reset identifiers
        resetMovementIdentifiers();

        // Check for AABB gameObject collisions
        List<float> correction = Collision.COL.checkCollisionAABB(player);

        // Handle AABB gameObject collisions
        handleCollisionsAABB(correction);       
    }


    void handleCollisionsAABB(List<float> correction)
    {
        // A horizontal collision occured
        if (correction[0] != 0)
        {
            // Apply correction to colliding objects
            transform.position = new Vector3(transform.position.x + correction[0], transform.position.y, transform.position.z);

            // Velocity should only be set to zero if the correction and velocity directions are opposite
            if (((correction[0] > 0) && (xVelocity < 0)) || ((correction[0] < 0) && (xVelocity > 0)))
                xVelocity = 0;
        }

        // A vertical collision occured
        if (correction[1] != 0)
        {
            // Apply correction to colliding objects
            transform.position = new Vector3(transform.position.x, transform.position.y + correction[1], transform.position.z);
            yVelocity = 0;

            // The player is currectly on the ground (correction moves player up)
            if (correction[1] > 0)
            {
                // Player is on the ground and not accelerating, apply friction
                if (!(Input.GetKey(GameManager.GM.right) || Input.GetKey(GameManager.GM.left)))
                    frictionGround = true;
                    
                airborne = false;
            }           
        }
    }

    void resetMovementIdentifiers()
    {
        airborne = true;
        frictionGround = false;
    }

    void applyPhysics()
    {
        // Apply gravity
        yVelocity = Physics.PHYS.calcGravity(yVelocity);

        // Apply ground friction
        if (frictionGround)
            xVelocity = Physics.PHYS.calcFriction(xVelocity);
    }

    void applyKeystrokes()
    {
        // Respond to "Left" key
        if (Input.GetKey(GameManager.GM.left))
            if (xVelocity > -xMax)
            {
                if (airborne)
                    xVelocity -= accelerationAirborne;

                else
                {
                    if (xVelocity > 0)
                        xVelocity -= accelerationPivot;

                    else
                        xVelocity -= acceleration;
                }                   
            }


        // Respond to "Right" key
        if (Input.GetKey(GameManager.GM.right))
            if (xVelocity < xMax)
            {
                if (airborne)
                    xVelocity += accelerationAirborne;

                else
                {
                    if (xVelocity < 0)
                        xVelocity += accelerationPivot;

                    else
                        xVelocity += acceleration;
                }                
            }


        // Respond to "Jump" key
        if (Input.GetKey(GameManager.GM.jump) && !airborne)
            yVelocity += .4f;
    }
}
