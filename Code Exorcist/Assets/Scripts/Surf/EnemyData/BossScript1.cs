using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript1 : MonoBehaviour
{
    //stats (for now lets just health to 15)
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //dead state (for now lets just destroy the game object
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            // a bullet has hit the boss
            health -= other.gameObject.GetComponent<BulletScript>().power;
            Destroy(other.gameObject);
        }
    }
}
