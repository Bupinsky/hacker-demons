using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //stats (for now this is a default bullet set at power 1)
    public int power;


    public Vector3 velocity;

    Terrain oceanSurface;
    float x;
    float y;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        oceanSurface = GameObject.FindWithTag("Ground").GetComponent<Terrain>();
    }

    // Update is called once per frame
    void Update()
    {
        x = transform.position.x + Time.deltaTime * velocity.x;
        z = transform.position.z + Time.deltaTime * velocity.z;
        //y = transform.position.y + Time.deltaTime * velocity.y;
        y = oceanSurface.SampleHeight(new Vector3(x, 0, z)) + oceanSurface.GetPosition().y;
        transform.position = new Vector3(x, y, z);

        // destroy out of bounds bullets
        if (!isInBounds())
        {
            Destroy(gameObject);
            //Debug.Log("bullet destroyed");
        }
    }

    public void SetVelocity(float xVel, float yVel, float zVel)
    {
        velocity = new Vector3(xVel, yVel, zVel);
    }

    public bool isInBounds()
    {
        if (z > 100)
        {
            return false;
        }
        return true;
    }
}
