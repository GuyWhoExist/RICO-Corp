using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //this manages all the buttons on the main menu - Nova

    private LevelProgressTracker levelProgressTracker;
    private int selectedLevelIndex;

    private void Awake()
    {
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        if (levelProgressTracker != null)
        {
            Debug.Log("We good in Main Menu Controller");
        }
        else
        {
            Debug.Log("Main Menu Controller can't find a Level Progress Tracker. womp womp");
        }
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("It worked ^q^");
        Debug.Log("Music should be playing (start button)");
        GameObject musicPlayer = GameObject.Find("Music Player");
        musicPlayer.GetComponent<MusicClass>().PlayMusic();
        SceneManager.LoadScene(2);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnLevelButtonClicked(LevelStatus levelStatus) //loads times associated with a level - Nova
    {
        if (levelStatus.unlocked)
        {
            selectedLevelIndex = levelStatus.GetLevelIndex();
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.hideFlags == HideFlags.None && obj.name == "Level Times") {
                    obj.SetActive(true);
                }
            }
            Debug.Log($"Starting Value: {levelStatus.GetLevelIndex()}");
            Debug.Log($"Checking Value: {levelStatus.GetLevelIndex() - 2}");
            foreach (LevelProgressTracker.LevelInfo l in levelProgressTracker.levels)
            {
                Debug.Log(l.levelIndex);
                Debug.Log(l.bestTime);
            }
            Debug.Log(levelProgressTracker.levels[levelStatus.GetLevelIndex() - 2].bestTime);
            if (levelProgressTracker.levels[levelStatus.GetLevelIndex()-2].bestTime == -1f)
            {
                foreach (GameObject obj in allObjects)
                {
                    if (obj.hideFlags == HideFlags.None)
                    {
                        if (obj.name == "Gold Time Text" || obj.name == "Silver Time Text" || obj.name == "Bronze Time Text")
                        {
                            obj.SetActive(false);
                        }
                        else if (obj.name == "Best Time Text")
                        {
                            obj.GetComponent<TextMeshProUGUI>().text = "Best Time: X:XX.X";
                        }
                    }
                }
            }
            else
            {
                foreach (GameObject obj in allObjects)
                {
                    if (obj.hideFlags == HideFlags.None)
                    {
                        if (obj.name == "Gold Time Text")
                        {
                            obj.SetActive(true);
                            obj.GetComponent<TextMeshProUGUI>().text = $"Gold: {levelProgressTracker.levels[levelStatus.GetLevelIndex() - 2].milestone3.ToString("0:00.00")}";
                        }
                        else if (obj.name == "Silver Time Text")
                        {
                            obj.SetActive(true);
                            obj.GetComponent<TextMeshProUGUI>().text = $"Silver: {levelProgressTracker.levels[levelStatus.GetLevelIndex() - 2].milestone2.ToString("0:00.00")}";
                        }
                        else if (obj.name == "Bronze Time Text")
                        {
                            obj.SetActive(true);
                            obj.GetComponent<TextMeshProUGUI>().text = $"Bronze: {levelProgressTracker.levels[levelStatus.GetLevelIndex() - 2].milestone1.ToString("0:00.00")}";
                        }
                        else if (obj.name == "Best Time Text")
                        {
                            obj.SetActive(true);
                            obj.GetComponent<TextMeshProUGUI>().text = $"Best Time: {levelProgressTracker.levels[levelStatus.GetLevelIndex() - 2].bestTime.ToString("0:00.00")}";
                        }
                    }
                }
            }

        }
        else
        {
            Debug.Log("Level Locked!");
        }
        
    }

    public void OnStartMissionButtonClicked() //starts the designated level - Nova
    {
        SceneManager.LoadScene(selectedLevelIndex);
    }


    public void OnLevelMenuButtonClicked() //the level select menu button, if it wasnt clear - Nova
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == "Levels")
            {
                obj.SetActive(true);
                /*foreach (GameObject l in obj.GetComponentsInChildren<GameObject>())
                {
                    LevelStatus status = l.GetComponent<LevelStatus>();
                    if (obj.GetComponent<LevelStatus>() != null)
                    {
                        status.UpdateStatus();
                    }
                }
                */
            }
        }
        GameObject startButton = GameObject.Find("Start Button");
        GameObject levelSelectButton = GameObject.Find("Level Select Button");
        startButton.SetActive(false);
        levelSelectButton.SetActive(false);
    }

    public void OnBackButtonClicked() //the back button for the level select - Nova
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == "Start Button")
            {
                obj.SetActive(true);
            }

        }
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == "Level Select Button")
            {
                obj.SetActive(true);
            }
        }
        GameObject levels = GameObject.Find("Levels");
        if (levels != null)
        {
            levels.SetActive(false);
        }
        GameObject times = GameObject.Find("Level Times");
        if (times != null)
        {
            times.SetActive(false);
        }
    }

}
