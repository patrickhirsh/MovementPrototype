using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a child of "AABB" in "Collidables" who's collision can be checked with checkCollisionAABB
// AABBCollidable objects store their half-dimensions for easy collision checking
public struct AABBCollidable
{
    public Vector3 halfDims;
    public GameObject gObject;

    // Creates a new Collidable object and calculates/stores its half-dimensions
    public AABBCollidable(GameObject gameObject)
    {
        gObject = gameObject;
        halfDims = gObject.GetComponent<Renderer>().bounds.size;

        // determine half-dimensions
        halfDims[0] = halfDims[0] / 2;
        halfDims[1] = halfDims[1] / 2;
        halfDims[2] = halfDims[2] / 2;
    }
}


public class Collision : MonoBehaviour
{

    public static Collision COL;

    List<AABBCollidable> AABBCollidables;


    // Singleton pattern
    void Awake()
    {
        if (COL == null)
        {
            DontDestroyOnLoad(gameObject);
            COL = this;
        }
        else if (COL != this)
            Destroy(gameObject);
    }


    // Use this for initialization
    void Start ()
    {
        generateCollidablesAABB();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    #region AABB Collision Detection

    // Takes an AABB Collidable object and checks if it's colliding with any other AABB Collidable objects
    // Returns a List containing two "correction" floats: (x correction, y correction)
    // These values should be added to the object's position to correct the collision
    // a collision entry of "0" indicates no collision of that type occured
    public List<float> checkCollisionAABB(AABBCollidable object1)
    {
        List<float> correction = new List<float>(new float[] { 0, 0 });

        foreach (AABBCollidable object2 in AABBCollidables)
        {
            float xMinDistance = object1.halfDims[0] + object2.halfDims[0];
            float xActualDistance = object1.gObject.transform.position[0] - object2.gObject.transform.position[0];

            // Is object1's x within collision distance of object 2?
            // NOTE: Positive xActualDistance = potential LEFT collision
            //       Negative xActualDistance = potential RIGHT collision
            if (Mathf.Abs(xActualDistance) < xMinDistance)
            {
                float yMinDistance = object1.halfDims[1] + object2.halfDims[1];
                float yActualDistance = object1.gObject.transform.position[1] - object2.gObject.transform.position[1];

                // Is object1's y within collision distance of object 2?
                // NOTE: Positive yActualDistance = potential FLOOR collision
                //       Negative yActualDistance = potential CEILING collision
                if (Mathf.Abs(yActualDistance) < yMinDistance)
                {

                    // COLLISION DETECTED //

                    float xOverlap = Mathf.Abs(xMinDistance - Mathf.Abs(xActualDistance));
                    float yOverlap = Mathf.Abs(yActualDistance - Mathf.Abs(yMinDistance));

                    // Ceiling/Floor collision detected
                    if (xOverlap >= yOverlap)
                    {
                        // Floor collision detected
                        if (yActualDistance > 0)
                            correction[1] = yOverlap;

                        // Ceiling collision detected
                        else
                            correction[1] = -yOverlap;
                    }

                    // Wall collision detected
                    else
                    {
                        // Left collision detected
                        if (xActualDistance > 0)
                            correction[0] = xOverlap;

                        // Right collision detected
                        else
                            correction[0] = -xOverlap;
                    }
                }
            }
        }

        return correction;
    }

    // Called on game start to initialize AABBCollidables
    // Takes all children of AABB and adds them to the AABBCollidables list
    void generateCollidablesAABB()
    {
        AABBCollidables = new List<AABBCollidable>();

        // Find all children of the AABB gameObject
        GameObject AABB = GameObject.Find("AABB");
        int children = AABB.transform.childCount;

        // Add children to collidablesAABBd
        for (int i = 0; i < children; i++)
        {
            AABBCollidables.Add(new AABBCollidable(AABB.transform.GetChild(i).gameObject));
        }
    }

    #endregion






}
