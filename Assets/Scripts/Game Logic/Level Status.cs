using UnityEngine;
using UnityEngine.UI;

public class LevelStatus : MonoBehaviour
{
    //I'm expecting to see a booleon that says if a level is completed. if it is then it will unlock the next level
    //if it's locked then it will stop looping through all of the levels
    private LevelProgressTracker levelProgressTracker;
    [SerializeField] private int levelNumber;
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
            Debug.Log("Level progress tracker found");
        }
        else
        {
            Debug.Log("Level progress tracker missing");
        }
        //Debug.Log(levelNumber - 2);

        if (levelNumber == 1) //unlocks levels
        {
            unlocked = true;
        }
        else if (levelProgressTracker.levels[levelNumber - 2].bestTime <= levelProgressTracker.levels[levelNumber - 2].milestone1 && levelProgressTracker.levels[levelNumber - 2].bestTime != -1f)
        {
            unlocked = true;
        }


        //Debug.Log(levelNumber);
        //Debug.Log(levelProgressTracker.levels[levelNumber - 1].bestTime);
        //Debug.Log(levelProgressTracker.levels[levelNumber - 1].milestone1);

        if (gameObject.GetComponent<Image>() != null)
        {
            if (levelProgressTracker.levels[levelNumber - 1].bestTime <= levelProgressTracker.levels[levelNumber - 1].milestone3 && levelProgressTracker.levels[levelNumber - 1].bestTime != -1f) //all goals done - Nova
            {
                //Debug.Log("Level " + levelNumber + " M3");
                gameObject.GetComponent<Image>().color = Color.yellow;
            }
            else if (levelProgressTracker.levels[levelNumber - 1].bestTime <= levelProgressTracker.levels[levelNumber - 1].milestone2 && levelProgressTracker.levels[levelNumber - 1].bestTime != -1f) //2 goals done - Nova
            {
                //Debug.Log("Level " + levelNumber + " M2");
                gameObject.GetComponent<Image>().color = Color.green;
            }
            else if (levelProgressTracker.levels[levelNumber - 1].bestTime <= levelProgressTracker.levels[levelNumber - 1].milestone1 && levelProgressTracker.levels[levelNumber - 1].bestTime != -1f) //minimum done - Nova
            {
                //Debug.Log("Level " + levelNumber + " M1");
                gameObject.GetComponent<Image>().color = Color.red;
            }
            else if (unlocked || cheat.unlockAll) //unlocked but havent played/beaten the level or cheats are enabled - Nova
            {
                //Debug.Log("Level "+levelNumber+" Unlocked");
                gameObject.GetComponent<Image>().color = Color.white;

            }
            else //not unlocked - Nova
            {
                //Debug.Log("Level " + levelNumber + " Locked");
                gameObject.GetComponent<Image>().color = Color.grey;
        }
            }
    }

    public int GetLevelIndex()
    {
        return levelNumber+1;
    }

    /*public void UpdateStatus()
    {
        

       
    }
    */


}
