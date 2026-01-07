using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//I know this is called RIFLE Enemy, but this can be used for any of the enemies. - Nova

public class RifleEnemy : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxSightDistance;
    [SerializeField] private float sightAngle;
    [SerializeField] private float attackAngle;
    [SerializeField] private float windupTime;
    [SerializeField] Transform player;
    [SerializeField] private float proximityDetection;
    [SerializeField] private float memoryLength;
    private float remembering;
    private Vector3 directionToPlayer;
    private float windupTimer;
<<<<<<< HEAD
    private EnemyState state;

=======
    private EnemyState state = EnemyState.IDLE;
>>>>>>> c2c46b94c977a853a2eeb9091f64e9a1cf0ed011
    private LineRenderer lR;
    private Vector3 localHit;
    private bool searching;

    private void Awake()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit sightHit, maxSightDistance);
        
        lR = GetComponent<LineRenderer>();
        lR.SetPosition(0, Vector3.zero);
        lR.SetPosition(1, new Vector3(0,0,maxSightDistance));
        lR.startColor = lR.materials[0].color;
    }
    [SerializeField] SightTracker sightTracker;
<<<<<<< HEAD


=======
>>>>>>> c2c46b94c977a853a2eeb9091f64e9a1cf0ed011
    private void Update()
    {
        UpdateState();
        RespondToState(state);
    }

    private void UpdateState() //changes the state of the enemy based on the players position - Nova
    {
        // default state is IDLE, but other conditions below may override this.
        Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, maxSightDistance);
        directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
        if (angleToPlayer < attackAngle && hit.transform == player)
        {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit3, maxSightDistance) &&
            hit3.transform == player) // detects if the player is in sight and right in front of the enemy - Nova
            {
                if (windupTimer < windupTime) //checks if the wind up for the attack is over or not - Nova
                {
                    Debug.Log("Winding Up");
                    state = EnemyState.WIND_UP;
                    sightTracker.kill = true;
                }
                else //if its over, we atac - Nova
                {
                    state = EnemyState.ATTACK;
                }
            }
        }
        else if (hit.transform == player && angleToPlayer < sightAngle || Vector3.Distance(player.position, transform.position) <= proximityDetection) //If player is in sight but not directly in front of the enemy - Nova
        {
            state = EnemyState.FOLLOW;
            //these modify the tracking cube on the UI - Nova
            sightTracker.kill = false;
            sightTracker.tracker.transform.LookAt(transform.position);
            //
        }
        else if (state == EnemyState.FOLLOW || searching == true) //if we are in the following state, but the conditions to enter follow arent true OR we are already in the searching state - Nova
        {
            if (!searching)
            {
                remembering = 0f;
                searching = true;
            }
            Debug.Log("I saw him! where did he go?");
            if (remembering >= memoryLength) //puts state back to idle if the timer ends - Nova
            {
                Debug.Log("I cant rember");
                state = EnemyState.IDLE;
            }
            else
            {
                state = EnemyState.SEARCHING;
            }
        }
    }
    private void RespondToState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE: //default state - Nova
                Debug.Log("whistling");
                searching = false;
                windupTimer = 0;
                break;
            case EnemyState.FOLLOW: //track the player - Nova
                Debug.Log("Found em!");
                searching = false;
                // rotate toward player
                windupTimer = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                break;
            case EnemyState.WIND_UP: //The player is in attack range, being winding up to kill - Nova
                searching = false;
                windupTimer += Time.deltaTime;
                break;
            case EnemyState.ATTACK: //KILL - Nova
                lR.useWorldSpace = true;
                lR.SetPosition(0, transform.position);
                lR.SetPosition(1, player.position);
                player.GetComponent<QuickRestart>().playerDie = true;
                Debug.Log("Bang bang bang, pull my devil trigger");
                windupTimer = 0;
                state = EnemyState.WIND_UP;
                break;
            case EnemyState.SEARCHING: //The enemy saw the player but they have left their sight. Keeps tracking the player but slower and for a limited time - Nova
                Debug.Log(remembering);
                remembering += Time.deltaTime;
                targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed/5 * Time.deltaTime);
                break;
        }
    }
}
