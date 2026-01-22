using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCamera : MonoBehaviour
{

    //the camera script we wrote together but modified for first person - Nova

    [SerializeField] private float sensitivity = 100f;
    [SerializeField] public Camera playerCamera;
    [SerializeField] private Transform playerBody;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private SprayPlacerHudController sprayPlacerHudController;
    [SerializeField] private TimerController endTracker;
    [SerializeField] public float FOV;
    public float storedFOV;
    public bool overflowBlock; //- allows to prevent the game enabling camera every single frame, alongside routing gamestops outside the script to stop related issues
    private Rigidbody rb;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector2 lookInput;
    private Controls inputActions;

    private void Awake()
    {
        overflowBlock = false;
        inputActions = new Controls();
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponentInParent<Rigidbody>();
        storedFOV = FOV;
    }

    //ctx => lookInput = Vector2.zero;
    private void OnEnable()
    {
        inputActions.Camera.Enable();
        inputActions.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); //This is effectively a compresed version of...
        //...this (
        inputActions.Camera.Look.canceled += Look_canceled;
    }
    private void Look_canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        lookInput = Vector2.zero;
    }
    //) but cant be reused and only does what is definded before
    private void OnDisable()
    {
        inputActions.Camera.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Camera.Look.canceled -= ctx => lookInput = Vector2.zero;
        inputActions.Camera.Disable();
    }

    private void Update()
    {
        Look(lookInput);

        // code to allow pause/levelend/spraymenu disabling cam movement
        if (pauseMenu.paused == true || endTracker.end == true ||  sprayPlacerHudController.selector == true)
        {
            inputActions.Camera.Disable();
            overflowBlock = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (pauseMenu.paused == false && overflowBlock == false || endTracker.end == false && overflowBlock == false || sprayPlacerHudController == false && overflowBlock == false)
        {
            inputActions.Camera.Enable();
            overflowBlock = true;
            Cursor.lockState = CursorLockMode.Locked;

            if (FOV != playerCamera.fieldOfView)
            {
                FOV = playerCamera.fieldOfView;
            }
        }
        //end
    }

    private void Look(Vector2 mouseInput)
    {
        float mouseX = mouseInput.x * sensitivity;
        float mouseY = mouseInput.y * sensitivity;

        // Pitch (up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Yaw (left/right)
        yRotation += mouseX;

        // Apply both rotations directly to the camera
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Quaternion deltaRotation = Quaternion.Euler(0f, mouseX, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

    }
}
