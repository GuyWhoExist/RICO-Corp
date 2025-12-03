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
    private Vector3 directionToPlayer;
    private float windupTimer;
    private EnemyState state;
    private LineRenderer lR;

    private void Awake()
    {
        lR = GetComponent<LineRenderer>();
    }
    [SerializeField] SightTracker sightTracker;

    private void Update()
    {
        UpdateState();
        RespondToState(state);
    }


    private void UpdateState() //changes the state of the enemy based on the players position - Nova
    {
        state = EnemyState.IDLE; // default state is IDLE, but other conditions below may override this.
        directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
        if (angleToPlayer < attackAngle)
        {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, maxSightDistance) &&
            hit.transform == player) // detects if the player is in sight and right in front of the enemy - Nova
            {
                if (windupTimer < windupTime) //checks if the wind up for the attack is over or not - Nova
                {
                    state = EnemyState.WIND_UP;
                    sightTracker.kill = true;
                }
                else //if its over, we atac = Nova
                {
                    state = EnemyState.ATTACK;
                }
            }
        }
        else if (angleToPlayer < sightAngle) //If player is in sight but not directly in front of the enemy - Nova
        {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, maxSightDistance) &&
            hit.transform == player)
            {
                state = EnemyState.FOLLOW;
                sightTracker.kill = false;
                sightTracker.tracker.transform.LookAt(transform.position);
            }
        }
    }
    private void RespondToState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                windupTimer = 0;
                break;
            case EnemyState.FOLLOW:
                // rotate toward player
                windupTimer = 0;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                break;
            case EnemyState.WIND_UP:
                windupTimer += Time.deltaTime;
                break;
            case EnemyState.ATTACK:
                lR.SetPosition(0, transform.position);
                lR.SetPosition(1, player.position);
                player.GetComponent<QuickRestart>().playerDie = true;
                Debug.Log("Bang bang bang, pull my devil trigger");
                windupTimer = 0;
                state = EnemyState.WIND_UP;
                break;
        }
    }
}
