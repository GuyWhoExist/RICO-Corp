using Unity.VisualScripting;
using UnityEngine;

public class LevelLoadTracker : MonoBehaviour
{
    private LevelProgressTracker levelProgressTracker;
    void Awake()
    {
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        levelProgressTracker.pauseMenu = null;
        levelProgressTracker.timerController = null;
        levelProgressTracker.initialComplete = false;
        levelProgressTracker.LevelLoaded();
        Debug.Log("level is loaded");
        Destroy(this.gameObject);
    }
}
