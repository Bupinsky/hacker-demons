using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Rigidbody rb;

    public float speed = 6f;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.Translate(moveDir.x * speed * Time.deltaTime, 0, moveDir.z * speed * Time.deltaTime);
        }
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector3(0, 7, 0), ForceMode.Impulse);
        }
    }
}
