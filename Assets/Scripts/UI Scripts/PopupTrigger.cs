using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private bool Enable;
    private void Start()
    {
        popUp.SetActive(false);
    }
    private void OnTriggerEnter(UnityEngine.Collider popup)
    {
       if (Enable)
        {
            if (popup.GetComponent<PlayerMovementTutorial>() != null)
            {
                popUp.SetActive(true);
                Destroy(gameObject);
            }
        }
       else
        {
            if (popup.GetComponent<PlayerMovementTutorial>() != null)
            {
                popUp.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
