using UnityEngine;

public class DestroyOnReplay : MonoBehaviour
{
    private LevelProgressTracker LPT;
    private DestroyOnReplay thisScript;
    void Start()
    {
        LPT = FindFirstObjectByType(typeof(LevelProgressTracker)) as LevelProgressTracker;
        thisScript = gameObject.GetComponent<DestroyOnReplay>();
        Invoke(nameof(CheckForReplay), 0.3f);
    }
    private void CheckForReplay()
    {
        if (LPT.levelCompleted == true)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(thisScript);
        }
    }
    //do not use this, this sets a very bad precident.
}
