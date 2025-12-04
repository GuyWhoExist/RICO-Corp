using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class PlayerMovementTutorial : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown; // Gee I wonder what these do :thinking: - Sawyer
    public float airMultiplier; //the modifier for when the player is in the air. (ex. 2 would double the players speed in the air. should be <= 1) - Nova
    bool readyToJump;
    [SerializeField] private float gravityMultiplier;
    [HideInInspector] public float walkSpeed; //this is effectively max speed. refer to this when needed - Nova
    [HideInInspector] public float sprintSpeed; //unused - Nova

    [Header("Sliding")]
    public float slideSpeed;
    public float slideYScale;
    public float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode slideKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [HideInInspector] public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Boost Config")]
    public float boostCoolDown;
    [SerializeField] private float killBoost;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private float storedSpeed;

    [HideInInspector] public bool sliding;

    [HideInInspector] public MovementState state;
    public enum MovementState
    {
        walking,
        sliding,
        air,
        diving
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        exitingSlope = false;

        readyToJump = true;
        startYScale = transform.localScale.y;
        storedSpeed = moveSpeed;

    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
            //Debug.Log("Grounded");
        }

        else
        {
            rb.linearDamping = 0;
            //Debug.Log("Air");
        }

        if (transform.position.y < -20f)
        {
            transform.position = spawn.transform.position;
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(slideKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            moveSpeed = 0f;
            moveDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;
        }

        if (Input.GetKeyUp(slideKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            moveSpeed = storedSpeed;
        }
    }

    private void StateHandler() //pretty self explanitory, changes the players state accordingly. mainly a debugging thing - Nova
    {
        
        if (Input.GetKey(slideKey))
        {
            if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround))
            {
                state = MovementState.sliding;
            }
            else
            {
                state = MovementState.diving;
            }
            state = MovementState.sliding;
            sliding = true;
            //moveDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;
        }
        else if (grounded && !Input.GetKey(slideKey)) //|| OnSlope())
        {
            state = MovementState.walking;
        }
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        if (moveSpeed != 0f)
        {
            // calculate movement direction
            moveDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;

            // on slope
            /*
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
                if (rb.linearVelocity.y > 0f)
                {
                    rb.AddForce(Vector3.down * 100f, ForceMode.Force);
                }
            }
            */

            // on ground
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }

            // in air
            else if (state == MovementState.air)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }

            /*
            if (!OnSlope())
            {
                rb.useGravity = true;
                //Debug.Log("Gravity Enabled");
            }
            else
            {
                rb.useGravity = false;
                //Debug.Log("Gravity Disabled");
            }
            */
        }

    }

    private void SpeedControl()
    {

        /*
        if (OnSlope())
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        
        else
        */
        //{
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        //}

    }

    private void Jump()
    {
        exitingSlope = true;
        // reset y velocity
        Debug.Log("Jumped");
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    /*public bool OnSlope() //this shit took too long to implement and it STILL doesn't work fully - Nova
    {

        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.6f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle < maxSlopeAngle && angle != 0)
            {
                //Debug.Log("Good slope");
            }
            return angle < maxSlopeAngle && angle != 0;
        }
        //Debug.Log("Bad slope");
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    */
   
}