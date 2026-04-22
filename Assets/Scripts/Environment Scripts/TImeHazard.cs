using UnityEngine;

public class TImeHazard : MonoBehaviour
{
    private QuickRestart quickRestart;
    private float timeDeath;
    private float avoidTimeStored;
    private bool contact;
    [SerializeField] float timeDeathTime;
    [SerializeField] float avoidTime;
    private bool planningMode;
    //coded by sawyer
    private void Awake()
    {
        quickRestart = FindAnyObjectByType<QuickRestart>();
        if (quickRestart == null)
            Debug.Log("WHAT ARE YOU DOING!? WHY IS THERE HAZARDS WITHOUT THE ACTUAL LEVEL!?");
        else
            Debug.Log("kill should function.");
        
        if (FindAnyObjectByType<PlanningModeController>() == true)
        {
            planningMode = true;
        }
        else
        {
            planningMode = false;
        }
            


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            contact = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent <PlayerMovementTutorial>() != null)
        {
            contact = false;
            avoidTimeStored = avoidTime;
        }
    }
    private void Update()
    {
        if (planningMode == false)
        {
            if (contact == true)
            {
                timeDeath -= Time.deltaTime;
            }
            if (contact == false)
            {
                avoidTimeStored -= Time.deltaTime;
            }
            if (avoidTimeStored < 0)
            {
                timeDeath = timeDeathTime;
            }
            if (timeDeath < 0)
            {
                quickRestart.PlayerDie();
            }
        }

    }
}
