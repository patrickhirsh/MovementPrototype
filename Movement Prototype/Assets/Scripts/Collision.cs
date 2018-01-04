using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AABBCollidable
{
    public Vector3 halfDims;
    public GameObject gObject;

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
        generateCollidables();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    // Called on game start to initialize AABBCollidables
    // Takes all children of AABB and adds them to the AABBCollidables list
    void generateCollidables()
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

    public List<float> checkCollisionAABB(AABBCollidable object1)
    {
        List<float> correction = new List<float>(new float[] {0, 0});

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
                            correction[1] = yOverlap + .0001f;

                        // Ceiling collision detected
                        else
                            correction[1] = -yOverlap - .0001f;              
                    }

                    // Wall collision detected
                    else
                    {
                        // Left collision detected
                        if (xActualDistance > 0)
                            correction[0] = xOverlap + .0001f;

                        // Right collision detected
                        else
                            correction[0] = -xOverlap - .0001f;
                    }
                }
            }
        }

        return correction;
    }




    #region AABB Collision Detection

    /*

    // Called on game start to initialize collidablesAABB
    // Takes all children of AABB (child of Collidables) and adds them to the collidablesAABB list
    void generateCollidablesAABB()
    {
        collidablesAABB = new List<GameObject>();

        // Find all children of the AABB gameObject
        GameObject AABB = GameObject.Find("AABB");
        int children = AABB.transform.childCount;

        // Add children to collidablesAABBd
        for (int i = 0; i < children; i++)
        {
            collidablesAABB.Add(AABB.transform.GetChild(i).gameObject);
        }
    }

    // Generates the collision bounds around a GameObject in format: (Left, Right, Up, Down)
    List<float> generateBounds(GameObject gObject)
    {
        return new List<float>(new float[]
        {
            gObject.transform.position.x - gObject.GetComponent<Renderer>().bounds.size[0] / 2,
            gObject.transform.position.x + gObject.GetComponent<Renderer>().bounds.size[0] / 2,
            gObject.transform.position.y + gObject.GetComponent<Renderer>().bounds.size[1] / 2,
            gObject.transform.position.y - gObject.GetComponent<Renderer>().bounds.size[1] / 2
        });
    }

    // Checks collision between the given gameObject and all children of the CollidablesAABB gameObject
    // returns two Gameobjects in the form: ( wall gameObject, floor/ceiling gameObject )
    // null gameObjects represent no collision of that type
    // non-null gameObjects are the objects which the given gObject collided with in that manor
    public List<GameObject> checkCollisionAABB(GameObject gObject)
    {
        // create output array
        GameObject verticalCollidable = null;       // wall collidable
        GameObject horizontalCollidable = null;     // floor/ceiling collidable
        List<GameObject> output = new List<GameObject>(new GameObject[]
        {
            verticalCollidable, horizontalCollidable
        });

        // check against all collidables for collision
        foreach(GameObject item in collidablesAABB)
        {
            if (compareBounds(gObject, item))
            {
                // Collision was with a wall
                if (collisionType(gObject, item) == "wall")
                    output[0] = item;

                // Collision was with a floor/ceiling
                else
                    output[1] = item;
            }
        }

        return output;
    }

    

    // Determines that some kind of collision between two objects occured
    bool compareBounds(GameObject object1, GameObject object2)
    {
        List<float> bounds1 = generateBounds(object1);
        List<float> bounds2 = generateBounds(object2);

        if ((bounds1[1] >= bounds2[0]) && (bounds1[0] <= bounds2[1]) &&     // Do the objects overlap in their vertical bounds?
            (bounds1[3] <= bounds2[2]) && (bounds1[2] >= bounds2[3]))       // Do the objects overlap in their horizontal bounds?
        {
            return true;
        }

        else
            return false;
    }

    // Determines whether the collision that occured was a wall or floor collision
    // returns "wall" for wall collision and "floor" for floor collision
    string collisionType(GameObject object1, GameObject object2)
    {
        List<float> bounds1 = generateBounds(object1);
        List<float> bounds2 = generateBounds(object2);

        // Find the region of object1 within the y bounds of object2
        float yLowerComparison;
        float yUpperComparison;
        float yDifference;

        if (bounds2[3] <= bounds1[3])
            yLowerComparison = bounds1[3];
        else
            yLowerComparison = bounds2[3];

        if (bounds2[2] >= bounds1[2])
            yUpperComparison = bounds1[2];
        else
            yUpperComparison = bounds2[2];

        yDifference = Mathf.Abs(yLowerComparison - yUpperComparison);


        // Find the region of object1 within the x bounds of object2
        float xLeftComparison;
        float xRightComparison;
        float xDifference;

        if (bounds2[0] <= bounds1[0])
            xLeftComparison = bounds1[0];
        else
            xLeftComparison = bounds2[0];

        if (bounds2[1] >= bounds1[1])
            xRightComparison = bounds1[1];
        else
            xRightComparison = bounds2[1];

        xDifference = Mathf.Abs(xRightComparison - xLeftComparison);

        // The collision was a floor collision
        if (xDifference >= yDifference)
            return "floor";

        // The collision was a wall collision
        else
            return "wall";
    }

    */

    #endregion






}
