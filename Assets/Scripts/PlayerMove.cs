using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;


//UNUSED, delete this script at some point - Nova


public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;

    Vector2 moveInput;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Run();
        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3 (0f, 0f, 0f);
        }
    }

    void Run()
    {
        Vector3 playerVelocity = new Vector3 (moveInput.x * speed, rb.angularVelocity.y, moveInput.y * speed);
        rb.angularVelocity = transform.TransformDirection(playerVelocity);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
