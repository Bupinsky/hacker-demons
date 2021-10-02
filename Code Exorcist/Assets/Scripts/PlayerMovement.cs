using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed;
    public float jumpForce;
    bool isGrounded = false ;
    public float gravityScale = 1;

    private Vector3 moveDirection;

    private void Start()
    {
        moveDirection = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirection.x = moveDir.x * speed;
            moveDirection.z = moveDir.z * speed;
        } else
        {
            moveDirection.x = 0f;
            moveDirection.z = 0f;
        }


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            moveDirection.y = jumpForce;
        }

        if (!isGrounded)
        {
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);
        }

        controller.Move(moveDirection * Time.deltaTime);


    }
    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 2f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
