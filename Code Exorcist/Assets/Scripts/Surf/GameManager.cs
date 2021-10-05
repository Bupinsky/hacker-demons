using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float increment;
  
    bool surfingStarted;

    public GameObject _Board;

    public GameObject RozXy;

    public GameObject OceanSurface;

    void Start()
    { 
        increment = .01f;
        surfingStarted = true;
        _Board.SetActive(true);
    }

    public void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            surfingStarted = true;
            _Board.SetActive(true);
        }

        //Exercise 14 requires that you write code to toggle the state of the WaveVanes on/off using the ToggleActiveState() method
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            OceanSurface.GetComponent<WaveVaneManager>().ToggleActiveState();
        }

        if (!surfingStarted)
        { 
           if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OceanSurface.GetComponent<WaveGeneration>().AdjustHeight(10);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OceanSurface.GetComponent<WaveGeneration>().AdjustHeight(-10);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OceanSurface.GetComponent<WaveGeneration>().ReRange(false);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OceanSurface.GetComponent<WaveGeneration>().ReRange(true);
            } 
      
            if (Input.GetKey(KeyCode.W))
            {
                OceanSurface.GetComponent<WaveGeneration>().AdjustSpeed(true);
            }

            if (Input.GetKey(KeyCode.S))
            {
                OceanSurface.GetComponent<WaveGeneration>().AdjustSpeed(false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                OceanSurface.GetComponent<WaveGeneration>().ShiftOriginate(-increment);
            }

            if (Input.GetKey(KeyCode.D))
            {
                OceanSurface.GetComponent<WaveGeneration>().ShiftOriginate(increment);
            }
        }

    }
}