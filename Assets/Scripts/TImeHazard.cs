using UnityEngine;

public class TImeHazard : MonoBehaviour
{
    private QuickRestart quickRestart;
    private float timeDeath;
    private float avoidTimeStored;
    private bool contact;
    [SerializeField] float timeDeathTime;
    [SerializeField] float avoidTime;
    //coded by sawyer
    private void Awake()
    {
        quickRestart = FindAnyObjectByType<QuickRestart>();
        if (quickRestart == null)
            Debug.Log("WHAT ARE YOU DOING!? WHY IS THERE HAZARDS WITHOUT THE ACTUAL LEVEL!?");
        else
            Debug.Log("kill should function.");
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
        if (contact == true)
        {
            timeDeath = timeDeath - Time.deltaTime;
        }
        if (contact == false)
        {
            avoidTimeStored = avoidTimeStored - Time.deltaTime;
        }
        if (avoidTimeStored < 0)
        {
            timeDeath = timeDeathTime;
        }
        if (timeDeath < 0)
        {
            quickRestart.playerDie = true;
        }
    }
}
