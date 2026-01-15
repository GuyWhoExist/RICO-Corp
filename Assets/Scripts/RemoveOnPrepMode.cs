using UnityEngine;

public class RemoveOnPrepMode : MonoBehaviour
{
    private void Awake()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            Destroy(gameObject);
    }
}

