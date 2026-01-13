
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Melee : MonoBehaviour
{
    private Controls controls;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject playerPosition;
    RaycastHit hit;
    [SerializeField] float meleeRange;
    [SerializeField] Shooting shooting;
    private Vector3 swingDirection;
    [SerializeField] private float swingCoolDown;
    [SerializeField] private PlayerMovementTutorial playerMovementTutorial;
    private float swingCoolDownStored;
    private AboveEnemy positionDetection;
    [SerializeField] private GameObject playerCamera;
    private PlayerMovementTutorial jumpHelper;
    private bool FOVIncrement;
    [SerializeField] float FOVShift;
    public bool meleeJump;
    //allows access to jumpforce
    //coded by sawyer


    private void Awake()
    {
        controls = new Controls();
        swingDirection = playerPosition.transform.forward;
        swingCoolDownStored = 0;
        jumpHelper = playerPosition.transform.GetComponent<PlayerMovementTutorial>();
    }
    private void OnEnable()
    {
       controls.Melee.Swing .Enable();
        controls.Melee.Swing.performed += Swing_performed;
    }


    private void OnTriggerEnter(Collider above)
    {
        if (above.GetComponent<AboveEnemy>() != null) 
        {
            positionDetection = above.GetComponent<AboveEnemy>();
            Debug.Log("I AM ABOVE");
        }

    }
    private void OnTriggerExit(Collider above)
    {
        positionDetection = null;
    }
    private void Swing_performed(InputAction.CallbackContext obj)
    {
        if (swingCoolDownStored < 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, meleeRange))
            {
                if (hit.transform.TryGetComponent(out IShootable shootable))
                {
                    
                     if (hit.transform.GetComponent<Enemy>() != null)
                      {
                        playerCamera.GetComponent<PlayerCamera>().FOV += FOVShift;
                        if (positionDetection != null)
                            {
                             playerPosition.transform.position = positionDetection.gameObject.transform.position;
                             rb.AddForce(transform.up * jumpHelper.jumpForce, ForceMode.Impulse);
                             meleeJump = true;
                            }
                        else
                            {
                             playerPosition.transform.position = shootable.GetGameObject().transform.position;
                             swingCoolDownStored = swingCoolDown;
                            }
                      }
                        
                  Destroy(shootable.GetGameObject());

                  Debug.Log("enemy SHOULD be bludgoned to death");

                  shooting.killStreak = shooting.killStreak + 1;
                  playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed + playerMovementTutorial.killBoost;
                  shooting.boostCoolDownStored = playerMovementTutorial.boostCoolDown;
              
                }
                Debug.Log("swing raycast is fired");
                FOVIncrement = true;
               
            }
            
            Debug.Log("melee is swung");
            
        }
    }

    private void OnDisable()
    {
        controls.Melee.Swing.Disable();
    }

    private void Update()
    {
        swingCoolDownStored = swingCoolDownStored - Time.deltaTime;
        if (FOVIncrement == true)
        {
            playerCamera.GetComponent<PlayerCamera>().FOV = playerCamera.GetComponent<PlayerCamera>().FOV - Time.deltaTime;
            if (playerCamera.GetComponent<PlayerCamera>().FOV <= playerCamera.GetComponent<PlayerCamera>().storedFOV)
            {
                playerCamera.GetComponent<PlayerCamera>().FOV = playerCamera.GetComponent<PlayerCamera>().storedFOV;
                FOVIncrement = false;
            }
        }

    }
    
}
