using Unity.VisualScripting;
using UnityEngine;

public class LevelLoadTracker : MonoBehaviour
{
    private LevelProgressTracker levelProgressTracker;
    void Awake()
    {
        FindAnyObjectByType<LevelProgressTracker>();
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        levelProgressTracker.pauseMenu = null;
        levelProgressTracker.timerController = null;
        levelProgressTracker.initialComplete = false;
        levelProgressTracker.levelLoaded = true;
        Debug.Log("level is loaded");
        Destroy(this.gameObject);
    }
}
