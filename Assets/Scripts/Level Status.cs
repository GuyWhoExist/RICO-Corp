using UnityEngine;
using UnityEngine.UI;

public class LevelStatus : MonoBehaviour
{
    private LevelProgressTracker levelProgressTracker;
    [SerializeField] private int levelIndex;
    public bool unlocked;
    private Cheats cheat;

    //i swear this script only works half the time - Nova

    private void Awake()
    {
        unlocked = false;
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        cheat = FindAnyObjectByType<Cheats>();
        if (levelProgressTracker != null)
        {
            Debug.Log("HEY, WE CAN TRACK LEVEL PROGRESS");
        }
        else
        {
            Debug.Log("Shit");
        }

        if (levelIndex == 1) //unlocks levels
        {
            unlocked = true;
        }
        else if (levelProgressTracker.levels[levelIndex - 2].bestTime <= levelProgressTracker.levels[levelIndex - 2].milestone1 && levelProgressTracker.levels[levelIndex - 2].bestTime != -1f)
        {
            unlocked = true;
        }


        Debug.Log(levelIndex);
        Debug.Log(levelProgressTracker.levels[levelIndex - 1].bestTime);
        Debug.Log(levelProgressTracker.levels[levelIndex - 1].milestone1);

        if (gameObject.GetComponent<Image>() != null)
        {
            if (levelProgressTracker.levels[levelIndex - 1].bestTime <= levelProgressTracker.levels[levelIndex - 1].milestone3 && levelProgressTracker.levels[levelIndex - 1].bestTime != -1f) //all goals done - Nova
            {
                Debug.Log("Level " + levelIndex + " M3");
                gameObject.GetComponent<Image>().color = Color.yellow;
            }
            else if (levelProgressTracker.levels[levelIndex - 1].bestTime <= levelProgressTracker.levels[levelIndex - 1].milestone2 && levelProgressTracker.levels[levelIndex - 1].bestTime != -1f) //2 goals done - Nova
            {
                Debug.Log("Level " + levelIndex + " M2");
                gameObject.GetComponent<Image>().color = Color.green;
            }
            else if (levelProgressTracker.levels[levelIndex - 1].bestTime <= levelProgressTracker.levels[levelIndex - 1].milestone1 && levelProgressTracker.levels[levelIndex - 1].bestTime != -1f) //minimum done - Nova
            {
                Debug.Log("Level " + levelIndex + " M1");
                gameObject.GetComponent<Image>().color = Color.red;
            }
            else if (unlocked || cheat.unlockAll) //unlocked but havent played or beaten the level - Nova
            {
                Debug.Log("Level "+levelIndex+" Unlocked");
                gameObject.GetComponent<Image>().color = Color.white;
            }
            else //not unlocked - Nova
            {
                Debug.Log("Level " + levelIndex + " Locked");
                gameObject.GetComponent<Image>().color = Color.grey;
            }
        }
    }

    public int GetLevelIndex()
    {
        return levelIndex+1;
    }

    /*public void UpdateStatus()
    {
        

       
    }
    */


}
