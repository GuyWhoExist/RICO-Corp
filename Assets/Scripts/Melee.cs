
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Melee : MonoBehaviour
{
    private Controls controls;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject playerPosition;
    [SerializeField] float meleeRange;
    [SerializeField] Shooting shooting;
    [SerializeField] private float swingCoolDown;
    [SerializeField] private PlayerMovementTutorial playerMovementTutorial;
    [SerializeField] private SpeedBoost speedBoost;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] float FOVShift;
    [SerializeField] float quickFallOffStored;
    private Vector3 swingDirection;
    private float swingCoolDownStored;
    private AboveEnemy positionDetection;
    private PlayerMovementTutorial jumpHelper;
    [HideInInspector] public bool teleportIncrement;
    [HideInInspector] public bool meleeJump;
    private float quickFallOff;
    public float maxModifiedFOV;
    private SightTracker trackerOfSight;
    private LevelProgressTracker levelProgressTracker;
    RaycastHit hit;
    private SightTracker sightTracker;

    //coded by sawyer

    [Header("hitstops")]
    private bool hitStopFire;
    private float hitStopDuration;
    [SerializeField] private float hitStopDurationStored;
    [SerializeField] GameObject hitStopLight;
    private GameObject storedEnemyHitStop;
    [SerializeField] AudioSource hitStopSFX;
    [SerializeField] AudioClip hitStopSFXAudio;



    private void Awake()
    {
        controls = new Controls();
        swingDirection = playerPosition.transform.forward;
        swingCoolDownStored = 0;
        jumpHelper = playerPosition.transform.GetComponent<PlayerMovementTutorial>();
        quickFallOff = quickFallOffStored;
        trackerOfSight = FindAnyObjectByType<SightTracker>();
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        hitStopLight.SetActive(false);
        sightTracker = FindAnyObjectByType<SightTracker>();
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
            //Debug.Log("I AM ABOVE");
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
                               if(levelProgressTracker.cheatsHitStopStatus)
                                {
                                    hitStopFire = true;
                                    hitStopDuration = hitStopDurationStored;
                                    Debug.Log("hitstop Triggered");
                                }
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
                        speedBoost.fuel += 0.5f;
                        //Debug.Log($"Fuel is at: {speedBoost.fuel}");
                        shooting.killStreak = shooting.killStreak + 1;
                        if (trackerOfSight.seen == true)
                        {
                            trackerOfSight.seen = false;
                        }
                    }
                    if (hitStopFire == true)
                    {
                        storedEnemyHitStop = shootable.GetGameObject();
                    }
                    else
                    {
                        sightTracker.seen = false;
                        Destroy(shootable.GetGameObject());
                    }

                 // Debug.Log("enemy SHOULD be bludgoned to death");
         
                }
                //Debug.Log("swing raycast is fired");
            }
            //Debug.Log("melee is swung");
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

        if (hitStopFire)
        {
            Debug.Log("hitstop");
            hitStopLight.SetActive(true);           
                Time.timeScale = 0;
                hitStopDuration -= Time.unscaledDeltaTime;
                if (hitStopDuration < 0)
                {

                    hitStopLight.SetActive(false);
                    Time.timeScale = 1;
                hitStopSFX.PlayOneShot(hitStopSFXAudio, 0.7f);
                hitStopFire = false;
                sightTracker.seen = false;
                    Destroy(storedEnemyHitStop);
                    storedEnemyHitStop = null;
                Debug.Log("hitstop end");
                }
        }
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
