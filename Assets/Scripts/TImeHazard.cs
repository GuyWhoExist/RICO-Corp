using UnityEngine;

public class TImeHazard : MonoBehaviour
{
    [SerializeField] QuickRestart quickRestart;
    private float timeDeath;
    private bool contact;
    [SerializeField] float timeDeathTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            contact = true;
            timeDeath = timeDeathTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent <PlayerMovementTutorial>() != null)
        {
            contact = false;
        }
    }
    private void Update()
    {
        if (contact == true)
        {
            timeDeath = timeDeath - Time.deltaTime;
        }

        if (timeDeath < 0)
        {
            quickRestart.playerDie = true;
        }
    }

}
