using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Board : MonoBehaviour
{
    Vector3 pos;
    Vector3 vel;
    Vector3 acc;
    float speed;

    float x, y, z;

    Vector3 gradient, normal;

    float g = 2f;  //
    Vector3 gravity;

    float characterSpeed = 50f;

    Vector3 origin;

    public Terrain oceanSurface;

    public float flatSurfaceValue;

    public GameObject bulletBlueprint;

    public List<GameObject> bullets;  

    public GameObject ocean;

    float count = 0f;

    float score = 0;

    bool dead = false;

    float cooldown = 0;

    public void Start()
    {
        acc = Vector3.zero;
        vel = Vector3.zero;

        gravity = new Vector3(0, -g, 0);

        x = 0f;
        z = 0f;
        y = oceanSurface.SampleHeight(new Vector3(x, 100f, z)) + oceanSurface.GetPosition().y;
        pos = new Vector3(x, y, z);
        transform.position = pos;

        normal = oceanSurface.GetComponent<TerrainCollider>().terrainData.GetInterpolatedNormal((x + 100f) / 200f, (z + 100f) / 200f).normalized;

        //not needed cuz I want it to start on a flat surface
        //transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, normal).normalized, normal);

        transform.Translate(0, .5f, 0, Space.Self);
       
    }


    public void Update()
    {
        count += Time.deltaTime;
        if (count > 1f)
        {
            ocean.GetComponent<WaveGeneration>().AdjustSpeed(true);
            count = 0f;
        }
        acc = Vector3.zero;  //reset, to start a new update cycle from scratch

        acc = Vector3.Dot(transform.forward, gravity) * transform.forward;
        vel = vel + Time.deltaTime * acc;
        //Surf();

        //make sure the board is facing forward
        transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);



        //////////////////////////////////////BASIC MOVEMENT/////////////////////////////////////

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
            {
                vel.z = characterSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                vel.z = -characterSpeed;
            }
        } else
        {
            vel.z = 0;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                vel.x = -characterSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                vel.x = characterSpeed;
            }
        }
        else
        {
            vel.x = 0;
        }

        //////////////////////////////////////////////////////////////////////////////////////////


        Surf();


        vel = vel + Time.deltaTime * acc;

        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i] == null)
            {
                bullets.RemoveAt(i);
                //Debug.Log("List item removed");
            }
        }

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        } else
        {
            cooldown = 0;

            if (Input.GetKey(KeyCode.Space) && cooldown <= 0)
            {
                Shoot("UP");
                cooldown = 0.4f;
            }


            // old controls
            /*            if (Input.GetKey(KeyCode.UpArrow) && cooldown <= 0)
                        {
                            Shoot("UP");
                            cooldown = 0.4f;
                        }
                        if (Input.GetKey(KeyCode.DownArrow) && cooldown <= 0)
                        {
                            Shoot("DOWN");
                            cooldown = 0.4f;
                        }
                        if (Input.GetKey(KeyCode.LeftArrow) && cooldown <= 0)
                        {
                            Shoot("LEFT");
                            cooldown = 0.4f;
                        }
                        if (Input.GetKey(KeyCode.RightArrow) && cooldown <= 0)
                        {
                            Shoot("RIGHT");
                            cooldown = 0.4f;
                        }*/
        }

        x = x + Time.deltaTime * (vel.x+vel.normalized.x);
        z = z + Time.deltaTime * (vel.z+vel.normalized.z); //subtract an ammount proportional to the movement of the waves

        // not gonna use this anymore probs
/*        if (normal.x > flatSurfaceValue || normal.z > flatSurfaceValue)
        {
            z -= ocean.GetComponent<WaveGeneration>().deltaZ * 50;
        }*/

            y = oceanSurface.SampleHeight(new Vector3(x, 0, z)) + oceanSurface.GetPosition().y;

        transform.position = new Vector3(x, y, z);

        normal = oceanSurface.GetComponent<TerrainCollider>().terrainData.GetInterpolatedNormal((x + 100f) / 200f, (z + 100f) / 200f).normalized;


        if (normal.x != 0 || normal.z != 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, normal).normalized, normal);
        } else
        {
            Debug.Log(normal);
        }

        transform.Translate(0, .5f, 0, Space.Self);
        
        if (transform.position.x > 100 || transform.position.x < -100 || transform.position.z > 100 || transform.position.z < -100)
        {
            PlayerPrefs.SetInt("score", (int)score);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (!dead)
        {
            score = Mathf.Floor((ocean.GetComponent<WaveGeneration>().deltaZ - 0.0002f) * 100000);
            if (PlayerPrefs.GetInt("highscore") <= score)
            {
                PlayerPrefs.SetInt("highscore", (int)score);
            }

        }

    }
    void Surf ()
    {
        gradient = new Vector3(normal.x, 0f, normal.z).normalized;

        if (normal.x > flatSurfaceValue || normal.z > flatSurfaceValue || -normal.x > flatSurfaceValue || -normal.z > flatSurfaceValue)
        {
            // old surf function
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(Quaternion.Euler(0, 90, 0) * gradient, normal).normalized, normal), 0.1f);
            /*vel.x += normal.x * normal.x * normal.x * 200;
            vel.z += normal.z * normal.z * normal.z * 200;*/

            // i think this is the part that determines traction
            vel.x += normal.x * normal.x * normal.x * 1000;
            vel.z += normal.z * normal.z * normal.z * 1000;

            //Debug.Log(gradient.x);
            //Debug.Log(normal.y);
            //Debug.Log(gradient.z);
        }
        // huh?
        // speed = vel.magnitude;


        // wha?
        // vel = speed * transform.forward;
    }

    void Shoot(string shotDirection)
    {
        if (shotDirection == "UP")
        {
            bullets.Add(Instantiate(bulletBlueprint, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity));
            bullets[bullets.Count - 1].transform.localScale = new Vector3(5, 5, 5);
            bullets[bullets.Count - 1].GetComponent<BulletScript>().SetVelocity(0, 0, 100);
        }
        if (shotDirection == "DOWN")
        {
            bullets.Add(Instantiate(bulletBlueprint, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity));
            bullets[bullets.Count - 1].transform.localScale = new Vector3(5, 5, 5);
            bullets[bullets.Count - 1].GetComponent<BulletScript>().SetVelocity(0, 0, -100);
        }
        if (shotDirection == "LEFT")
        {
            bullets.Add(Instantiate(bulletBlueprint, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity));
            bullets[bullets.Count - 1].transform.localScale = new Vector3(5, 5, 5);
            bullets[bullets.Count - 1].GetComponent<BulletScript>().SetVelocity(-100, 0, 0);
        }
        if (shotDirection == "RIGHT")
        {
            bullets.Add(Instantiate(bulletBlueprint, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity));
            bullets[bullets.Count - 1].transform.localScale = new Vector3(5, 5, 5);
            bullets[bullets.Count - 1].GetComponent<BulletScript>().SetVelocity(100, 0, 0);
        }
    }

    void OnGUI()
    {

        GUI.color = Color.white;
        GUI.skin.box.fontSize = 15;
        GUI.skin.box.wordWrap = false;


        GUI.Box(new Rect(10, 10, 300, 30), "Score: " + score);
        GUI.Box(new Rect(10, 40, 300, 30), "Highscore: " + PlayerPrefs.GetInt("highscore").ToString());
    }
}
