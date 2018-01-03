using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{

    public static Physics PHYS;

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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
