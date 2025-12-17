using UnityEngine;

public class Sludge : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
       if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed /= 2;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed *= 2;
        }
    }
}
