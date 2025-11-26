using Unity.VisualScripting;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] QuickRestart quickRestart;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            quickRestart.playerDie = true;
        }
    }
}
