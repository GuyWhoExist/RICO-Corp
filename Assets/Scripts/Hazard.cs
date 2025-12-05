using Unity.VisualScripting;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private QuickRestart quickRestart;
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
            quickRestart.playerDie = true;
        }
    }
}
