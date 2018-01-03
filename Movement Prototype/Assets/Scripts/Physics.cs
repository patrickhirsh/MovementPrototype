﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{

    public static Physics PHYS;

    float fps = 60;
    float coFriction = .25f;
    float gravity = 1.2f;


    // On game start
    void Awake()
    {
        // Ensure we only ever have one GameManager object
        if (PHYS == null)
        {
            DontDestroyOnLoad(gameObject);
            PHYS = this;
        }
        else if (PHYS != this)
        {
            Destroy(gameObject);
        }
	}

	public float calcGravity(float yVelocity)
	{
		return yVelocity - (gravity / fps);
	}

    public float CalcFriction(float xVelocity)
    {
        return xVelocity + (coFriction * gravity)/60;
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
