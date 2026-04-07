using UnityEngine;

public class CreateOnPrepMode : MonoBehaviour
{
    private void Awake()
    {
        if (FindAnyObjectByType<PlanningModeController>() == false)
            Destroy(gameObject);
    }
}
