using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistRockRoll : MonoBehaviour
{
    public enum Pose { NORTH, SOUTH, WEST, EAST };

    Pose pose;

    void Start()
    {
        pose = Pose.NORTH;
    }

    public void Twist(Pose wayToFace)
    {
        pose = wayToFace;
        switch(pose)
        {
            case Pose.NORTH:
               transform.localRotation = Quaternion.identity;
               break;
            case Pose.SOUTH:
               transform.localRotation = Quaternion.Euler(0, 180, 0);
               break;
            case Pose.EAST:
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
            case Pose.WEST:
                transform.localRotation = Quaternion.Euler(0, -90, 0);
                break;
        }
    }

    public void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.W)) //SpeedUp
        {
            switch (pose)
            {
                case Pose.NORTH:
                    transform.Rotate(+15f, 0f, 0f, Space.Self);
                    break;
                case Pose.SOUTH:
                    transform.Rotate(-15f, 0f, 0f, Space.Self);
                    break;
                case Pose.EAST:
                    transform.Rotate(0f, 0f, +15f, Space.Self);
                    break;
                case Pose.WEST:
                    transform.Rotate(0f, 0f, -15f, Space.Self);
                    break;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.S))  //SlowDown
        {
            switch (pose)
            {
                case Pose.NORTH:
                    transform.Rotate(-15f, 0f, 0f, Space.Self);
                    break;
                case Pose.SOUTH:
                    transform.Rotate(+15f, 0f, 0f, Space.Self);
                    break;
                case Pose.EAST:
                    transform.Rotate(0f, 0f, -15f, Space.Self);
                    break;
                case Pose.WEST:
                    transform.Rotate(0f, 0f, +15f, Space.Self);
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.A)) //TurnLeft
        {
            switch (pose)
            {
                case Pose.NORTH:
                    transform.Rotate(0f, 0f, +15f, Space.Self);
                    break;
                case Pose.SOUTH:
                    transform.Rotate(0f, 0f, -15f, Space.Self);
                    break;
                case Pose.EAST:
                    transform.Rotate(-15f, 0f, 0f, Space.Self);
                    break;
                case Pose.WEST:
                    transform.Rotate(+15f, 0f, 0f, Space.Self);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.D)) //TurnRight
        {
            switch (pose)
            {
                case Pose.NORTH:
                    transform.Rotate(0f, 0f, -15f, Space.Self);
                    break;
                case Pose.SOUTH:
                    transform.Rotate(0f, 0f, +15f, Space.Self);
                    break;
                case Pose.EAST:
                    transform.Rotate(+15f, 0f, 0f, Space.Self);
                    break;
                case Pose.WEST:
                    transform.Rotate(-15f, 0f, 0f, Space.Self);
                    break;
            }
        }


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)) //SpeedUp
        {
            Twist(pose);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
           Twist(TwistRockRoll.Pose.NORTH);
            //transform.localRotation = Quaternion.identity;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Twist(TwistRockRoll.Pose.SOUTH);
            //transform.localRotation = Quaternion.Euler(0,180, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Twist(TwistRockRoll.Pose.WEST);
            //transform.localRotation = Quaternion.Euler(0, -90, 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Twist(TwistRockRoll.Pose.EAST);
            //transform.localRotation = Quaternion.Euler(0, 90, 0);
        }

    }
}

