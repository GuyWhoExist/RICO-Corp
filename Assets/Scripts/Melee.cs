
using System;
using Unity.VisualScripting;
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
    [HideInInspector] public bool teleportIncrement;
    [SerializeField] float FOVShift;
    [HideInInspector]public bool meleeJump;
    private float quickFallOff;
    [SerializeField] float quickFallOffStored;
    public float maxModifiedFOV;
    private SightTracker trackerOfSight;
    //allows access to jumpforce
    //coded by sawyer


    private void Awake()
    {
        controls = new Controls();
        swingDirection = playerPosition.transform.forward;
        swingCoolDownStored = 0;
        jumpHelper = playerPosition.transform.GetComponent<PlayerMovementTutorial>();
        quickFallOff = quickFallOffStored;
        trackerOfSight = FindAnyObjectByType<SightTracker>();
    }
    private void OnEnable()
    {
     if (FindAnyObjectByType<PlanningModeController>() == false)
        {
            controls.Melee.Swing.Enable();
            controls.Melee.Swing.performed += Swing_performed;
        }
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
                            Camera.main.fieldOfView += FOVShift * 2;
                            teleportIncrement = true;
                        }
                      }


                    if (hit.transform.GetComponent<Enemy>() != null)
                    {
                        shooting.killStreak = shooting.killStreak + 1;
                        if (trackerOfSight.seen == true)
                        {
                            trackerOfSight.seen = false;
                        }
                        playerMovementTutorial.moveSpeed += playerMovementTutorial.killBoost;
                        shooting.boostCoolDownStored = playerMovementTutorial.boostCoolDown;
                    }
                    Destroy(shootable.GetGameObject());
                  Debug.Log("enemy SHOULD be bludgoned to death");
         
                }
                Debug.Log("swing raycast is fired");
                
               
            }
            
            Debug.Log("melee is swung");
            
        }
    }

    private void OnDisable()
    {
        controls.Melee.Swing.Disable();
        controls.Melee.Swing.performed -= Swing_performed;
    }

    private void Update()
    {
        swingCoolDownStored = swingCoolDownStored - Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (teleportIncrement)
        {
            quickFallOff -= Time.deltaTime;
            if (quickFallOff < 0)
            {
                Camera.main.fieldOfView -= FOVShift / 4;
                quickFallOff = quickFallOffStored;
                if (Camera.main.fieldOfView < playerCamera.GetComponent<PlayerCamera>().storedFOV + maxModifiedFOV)
                    teleportIncrement = false;
            }
                
        }
            
    }
}
