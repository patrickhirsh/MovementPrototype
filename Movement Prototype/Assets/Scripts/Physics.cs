using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{

    public static Physics PHYS;

    // Physics constants
    float FPS = 60;
    float CO_FRICTION = .7f;
    float GRAVITY = .9f;


    // Singleton pattern
    void Awake()
    {
        if (PHYS == null)
        {
            DontDestroyOnLoad(gameObject);
            PHYS = this;
        }
        else if (PHYS != this)
            Destroy(gameObject);
	}

    // Adjusts yVelocity for gravity
	public float calcGravity(float yVelocity)
	{
		return yVelocity - (GRAVITY / FPS);
	}

    // Adjusts xVelocity for friction
    public float calcFriction(float xVelocity)
    {
        if (xVelocity > .01)
            return xVelocity - (CO_FRICTION * GRAVITY) / FPS;

        else if (xVelocity < -.01)
            return xVelocity + (CO_FRICTION * GRAVITY) / FPS;

        else
            return xVelocity = 0;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
