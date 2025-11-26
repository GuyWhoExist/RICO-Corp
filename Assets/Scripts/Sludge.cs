using UnityEngine;

public class Sludge : MonoBehaviour
{
    [SerializeField] PlayerMovementTutorial playerMovement;

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            playerMovement.moveSpeed = playerMovement.moveSpeed / 2;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            playerMovement.moveSpeed = playerMovement.moveSpeed * 2;
        }
    }


}
