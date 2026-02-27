using UnityEngine;

public class LevelLoadTracker : MonoBehaviour
{
    private LevelProgressTracker levelProgressTracker;
    void OnEnable()
    {
        FindAnyObjectByType<LevelProgressTracker>();
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        levelProgressTracker.levelLoaded = true;
        Debug.Log("level is loaded");
        Destroy(this.gameObject);
    }
}
