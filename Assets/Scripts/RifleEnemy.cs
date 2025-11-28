using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
    [SerializeField] SightTracker sightTracker;

    private void Update()
    {
        UpdateState();
        RespondToState(state);
    }


    private void UpdateState()
    {
        state = EnemyState.IDLE; // default state is IDLE, but other conditions below may override this.
        directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
        if (angleToPlayer < attackAngle)
        {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, maxSightDistance) &&
            hit.transform == player)
            {
                if (windupTimer < windupTime)
                {
                    state = EnemyState.WIND_UP;
                    sightTracker.kill = true;
                }
                else
                {
                    state = EnemyState.ATTACK;
                }
            }
        }
        else if (angleToPlayer < sightAngle)
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
                // glow red, increase timer
                windupTimer += Time.deltaTime;
                break;
            case EnemyState.ATTACK:
                player.GetComponent<QuickRestart>().playerDie = true;
                Debug.Log("Archer attack!");
                windupTimer = 0;
                state = EnemyState.WIND_UP;
                break;
        }
    }
}
