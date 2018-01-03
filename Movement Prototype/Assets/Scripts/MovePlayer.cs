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

    // Hardcoded collision Vector
    List<List<float>> collidables = new List<List<float>>();

    // Use this for initialization
    void Start ()
    {
        // Add hardcoded collidables
        collidables.Add(new List<float>(new float[] { 0, 0, 100, 0 }));
        collidables.Add(new List<float>(new float[] { -5, 2, 1, 4 }));
        collidables.Add(new List<float>(new float[] { 5, 1, 4, 2 }));
    }
	

    // Update is called once per frame
    void Update()
    {
        float playerLeftbound = transform.position.x - .5f;
        float playerRightbound = transform.position.x + .5f;
        float playerUpperbound = transform.position.y + .5f;
        float playerLowerbound = transform.position.y - .5f;

        bool xLeftCollision = false;
        bool xRightCollision = false;
        bool yUpperCollision = false;
        bool yLowerCollision = false;

        bool rValid = true;
        bool lValid = true;
        bool jValid = true;

        foreach (List<float> item in collidables)
        {
            float itemLeftbound = item[0] - (item[2] / 2);
            float itemRightbound = item[0] + (item[2] / 2);
            float itemUpperbound = item[1] + (item[3] / 2);
            float itemLowerbound = item[1] - (item[3] / 2);

            // Left collision
            if ((playerLeftbound <= itemRightbound) && (playerLeftbound >= itemLeftbound))
            {
                print("Holiday");
                xLeftCollision = true;
            }

            // Right collision
            if ((playerRightbound >= itemLeftbound) && (playerRightbound <= itemRightbound))
            {
                xRightCollision = true;
            }

            // Upper collision
            if ((playerUpperbound >= itemLowerbound) && (playerUpperbound <= itemUpperbound))
            {
                yUpperCollision = true;
            }

            // Lower collision
            if ((playerLowerbound <= itemUpperbound) && (playerLowerbound >= itemLowerbound))
            {
                yLowerCollision = true;
            }
        }

        if (xLeftCollision)
        {
            xVelocity = 0;
            lValid = false;
        }

        if (xRightCollision)
        {
            xVelocity = 0;
            rValid = false;
        }

        // Stop y movement on vertical collision & apply friction
        if (yUpperCollision || yLowerCollision)
        {
            yVelocity = 0;
            xVelocity = Physics.PHYS.calcFriction(xVelocity);
        }

        // If airborne, apply gravity
        if (!yLowerCollision)
        {
            yVelocity = Physics.PHYS.calcGravity(yVelocity);
            jValid = false;
        }

        // Respond to key events
        if (Input.GetKey(GameManager.GM.left) && lValid)
        {
            if (xVelocity > -xMax)
            {
                xVelocity = xVelocity - .01f;
            }
        }
        if (Input.GetKey(GameManager.GM.right) && rValid)
        {
            if (xVelocity < xMax)
            {
                xVelocity = xVelocity + .01f;
            }
        }
        if (Input.GetKey(GameManager.GM.jump) && jValid)
        {
            transform.position += Vector3.up / 2;
        }

        // Apply velocity change
        transform.position = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, transform.position.z);
    }
}
