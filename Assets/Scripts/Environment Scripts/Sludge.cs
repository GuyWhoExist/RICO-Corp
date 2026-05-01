using UnityEngine;

public class Sludge : MonoBehaviour
{
    //coded by sawyer
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided with");
       if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed /= 2;
            Debug.Log(collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed);

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("uncollided with");
        if (collision.transform.GetComponent<PlayerMovementTutorial>() != null)
        {
            collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed *= 2;
            Debug.Log(collision.transform.GetComponent<PlayerMovementTutorial>().moveSpeed);
        }
    }
}
