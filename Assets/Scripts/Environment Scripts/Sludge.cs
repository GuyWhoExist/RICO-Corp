using UnityEngine;

public class Sludge : MonoBehaviour
{
    //coded by sawyer
    private void OnCollisionEnter(Collision collision)
    {
       if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
       {
            Debug.Log("Entered sludge");
            collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed /= 2;
       }
    }
    private void OnCollisionExit(Collision collision)
    {
       if(collision.transform.GetComponent<PlayerMovementTutorial>() != null)
       {
            Debug.Log("Exited sludge");
            collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed *= 2;
       }
    }
}
