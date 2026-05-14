using UnityEngine;

public class RemoveWallForPlanningTutorial : MonoBehaviour
{
    private Spray sprayLogger;
    void Start()
    {
        if (FindFirstObjectByType<PlanningModeController>())
        {
            Destroy(gameObject);
        }
        else
        {
            Invoke(nameof(LogSprays), 0.3f);
        }
    }
    private void LogSprays()
    {
        if (FindFirstObjectByType<Spray>())
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject.GetComponent<RemoveWallForPlanningTutorial>());
        }
    }

    //do not use this, this sets a very bad precident.
}
