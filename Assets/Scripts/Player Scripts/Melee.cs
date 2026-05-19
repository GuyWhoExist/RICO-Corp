
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : MonoBehaviour
{
    private Controls controls;//used to take input from the new input sysetm
    [SerializeField] Rigidbody rb;//the players rigidbody
    [SerializeField] float meleeRange;//the range of the melee swing
    [SerializeField] Shooting shooting;//the shooting script
    [SerializeField] private float swingCoolDown;//the cooldown between melee swings
    [SerializeField] private SpeedBoost speedBoost;//the speed boost functionality
    [SerializeField] private GameObject playerCamera;//the players camera script
    [SerializeField] float FOVShift;//the fov alteration value
    [SerializeField] float quickFallOffStored;//used to avoid issues with jarring fov shifting
    [SerializeField] private ChromaticAberrationEffect cAEffect;//the chromatic aberration effect script
    private Vector3 swingDirection;//the direction the player will swing the melee in
    private float swingCoolDownStored;//the stored value of the swing cooldown
    private AboveEnemy positionDetection;//the scripted object used for detecting jumping off an enemies head
    private PlayerMovementTutorial jumpHelper;//the player movement script, used for jumping off enemies
    private PlayerCamera hitStopControllInfo;//the player camera script
    [HideInInspector] public bool teleportIncrement;//used to teleport the player to the enemy on a default melee hit
    [HideInInspector] public bool meleeJump;//used to tell other scripts the player has jumped after performing a melee above an enemy
    private float quickFallOff;//the raw value of quickFallOffStored
    public float maxModifiedFOV;//the maximum the FOV can shift to
    private PlanningModeController planningModeController;//the planning mode controller to avoid melee activating in planning mode
    RaycastHit hit;//the hit point of the melee swing

    //coded by sawyer
    //functions include: the melee, melee bouncing, the melee boost, and hitstops.

    [Header("hitstops")]
    private bool hitStopFire;//used to tell other scripts that a hitstop is occuring
    private float hitStopDuration;//used to dictate the hit stop's duration internally 
    [SerializeField] private float hitStopDurationStored;//used for coders to manually set the hitstop length
    [SerializeField] GameObject hitStopLight;//the effect that shows up when the hitstop fires
    private GameObject storedEnemyHitStop;//the enemy that the hitstop is targeting
    [SerializeField] AudioSource hitStopSFX;//the audio effect player for the hitstop
    [SerializeField] AudioClip hitStopSFXAudio;//the audio clip for the hitstop sound effects

    private void Awake()//start of awake
    {
        controls = new Controls();//sets the controls value
        swingDirection = this.transform.forward;//sets the swing direction
        swingCoolDownStored = 0;//clears the cooldown vale
        jumpHelper = this.transform.GetComponent<PlayerMovementTutorial>();//used to get the player movement script
        quickFallOff = quickFallOffStored;//sets the quickfalloff value
        hitStopControllInfo = FindAnyObjectByType<PlayerCamera>();//used to get th eplayer camera script
        planningModeController = FindAnyObjectByType<PlanningModeController>();//tries to get the planning mode controller, if it is there, then it is in planning mode
        hitStopLight.SetActive(false);//disables the hitstop effect object
    }//end of awake
    private void OnEnable()//start of OnEnable
    {
        if (planningModeController == null)//checks if the player is within planning mode
        {
            controls.Melee.Swing.Enable();//enables the melee swing input
            controls.Melee.Swing.performed += Swing_performed;//adds the swing performed input
        }
     
    }//end of OnEnable
    private void OnDisable()//start of OnDisable
    {
        controls.Melee.Swing.Disable();//disables the melee swing input
        controls.Melee.Swing.performed -= Swing_performed;//removes the swing performed input
    }//end of OnDisable
    private void OnTriggerEnter(Collider above)//start of OnTriggerEnter
    {
        if (above.GetComponent<AboveEnemy>() != null) //checks if the player has entered the above enemy detection trigger
        {
            positionDetection = above.GetComponent<AboveEnemy>();//if they are, marks the player as above the enemy
        }

    }//end of OnTriggerEnter
    private void OnTriggerExit(Collider above)//start of OnTriggerExit
    {
        positionDetection = null;//clears the position detector
    }//end of OnTriggerExit
    private void Swing_performed(InputAction.CallbackContext obj)//the start of the swing performed input
    {
        if (swingCoolDownStored < 0)///chekcs if the swing cooldown has ended
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, meleeRange))//fires a raycast at the length of the melee range
            {
                if (hit.transform.TryGetComponent(out IShootable shootable))//checks if the hit object is destroyable
                {
                    if (hit.transform.GetComponent<Enemy>() != null)//if it is an enemy
                    {
                        speedBoost.fuel += 0.5f;//increase the speed boost charges by half a charge
                        shooting.storedEnemy = hit.transform.GetComponent<RifleEnemy>();//stores the enemy in shooting if the enemy could attack the player
                       

                        //Debug.Log($"Fuel is at: {speedBoost.fuel}");
                        shooting.killStreak = shooting.killStreak + 1;//increases killstreak by 1

                        if (positionDetection != null)//used to detect a midair melee to trigger a melee jump
                        {
                            HitStop();//fires the hitstop effect
                            hitStopFire = true;//marks the hitstop bool as true for internal info checks
                            hitStopDuration = hitStopDurationStored;//sets the hitstop duration
                            //Debug.Log("hitstop Triggered");

                            this.transform.position = positionDetection.gameObject.transform.position;//sets the players position to the jump triggers position
                            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);//resets the players vertical velocity
                            rb.AddForce(transform.up * jumpHelper.jumpForce, ForceMode.Impulse);//impulses the player up with the force of a standard jump
                            meleeJump = true;//marks the melee jump as having occured
                        }
                        else
                        {
                            if (hit.transform.GetComponent<Enemy>().meleeImmune == false)//checks if the enemy is immune to melee
                            {
                                this.transform.position = shootable.GetGameObject().transform.position;//if they are, do not destroy the enemy, only apply the movement effects
                            }
                            swingCoolDownStored = swingCoolDown;
                            Camera.main.fieldOfView += FOVShift * 2;
                            teleportIncrement = true;
                        }
                        shooting.EnemyKill();
                    }
                    else if (hit.transform.GetComponent<Absorb>())
                    {
                        hit.transform.GetComponent<Absorb>().MeleedB();
                    }
                    else if (hit.transform.GetComponent<Destroyable>())
                    {
                        hit.transform.GetComponent<Destroyable>().MeleedA();
                    }

                    if (hitStopFire == true)
                    {
                        storedEnemyHitStop = shootable.GetGameObject();
                    }
                    else
                    {
                        if (shootable.GetGameObject().GetComponent<Enemy>() != null && shootable.GetGameObject().GetComponent<Enemy>().meleeImmune != true)
                        {
                            Debug.Log("Not a tutorial enemy (melee check)");
                            Destroy(shootable.GetGameObject());
                        }
                        else
                        {
                            Debug.Log("tutorial enemy (melee check)");
                        }
                    }

                 // Debug.Log("enemy SHOULD be bludgoned to death");
         
                }
                //Debug.Log("swing raycast is fired");
            }
            //Debug.Log("melee is swung");
        }
    }//the end of the swing performed input

    private void Update()// start of update
    {
        swingCoolDownStored = swingCoolDownStored - Time.deltaTime;
        hitStopDuration -= Time.unscaledDeltaTime;
        if (hitStopDuration < 0 && storedEnemyHitStop != null)
        {
            HitStopEnd();
        }
    }//end of update

    private void HitStop()//start of the hitstop function
    {
        cAEffect.Activate();//effects the chromatic aberration effect
        hitStopControllInfo.Freeze();//freezes player inputs
            hitStopLight.SetActive(true);//enables the hitstop effect object
            controls.Melee.Swing.Disable();//disables the melee swing
    }//end of the hitstop function

    private void HitStopEnd()//the start of the hitstop end function
    {
        hitStopLight.SetActive(false);
        hitStopControllInfo.Unfreeze();
        hitStopSFX.PlayOneShot(hitStopSFXAudio, 0.7f);
        hitStopFire = false;
        controls.Melee.Swing.Enable();
        swingCoolDownStored = swingCoolDown;
        if (storedEnemyHitStop.GetComponent<Enemy>() != null && storedEnemyHitStop.GetComponent<Enemy>().bounceImmune != true)
        {
            Debug.Log("Not a tutorial enemy (bounce check)");
            shooting.EnemyKill();
            Destroy(storedEnemyHitStop);
        }
        else
        {
            Debug.Log("tutorial enemy (bounce check)");
        }
        
        storedEnemyHitStop = null;
    }//the end of the hitstop end function
    

   
    private void LateUpdate()//start of lateupdate
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

    }//end of lateupdate
}
