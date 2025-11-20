using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //this manages all the buttons on the main menu - Nova

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

    public void OnLevelButtonClicked(LevelStatus levelStatus)
    {
        if (levelStatus.unlocked)
        {
            GameObject musicPlayer = GameObject.Find("Music Player");
            if (musicPlayer != null)
            {
                Debug.Log("Music SHOULD be playing (Level Select button)");
                musicPlayer.GetComponent<MusicClass>().PlayMusic();
            }
            else
            {
                Debug.Log("Music Player not found");
            }
            SceneManager.LoadScene(levelStatus.GetLevelIndex());
        }
        else
        {
            Debug.Log("Level Locked!");
        }
        
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
        levels.SetActive(false);
    }

}
