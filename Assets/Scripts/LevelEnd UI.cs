using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//Used to manipulate and edit the text on the level ending UI - Nova
// joint effort -sawyer
public class LevelEndUI : MonoBehaviour
{
    private LevelProgressTracker levelProgressTracker;
    private TimerController timerController;
    private LevelEnder levelEnder;
    [SerializeField] TextMeshProUGUI resultsDisplay;
    [SerializeField] GameObject endGUI;

    private void Awake()
    {
        levelEnder = FindAnyObjectByType<LevelEnder>();
        if (levelEnder == null)
            Debug.Log("hey, end the level");
        else
            Debug.Log("level is endeable");

        endGUI.SetActive(false);
        //timerController = GetComponent<TimerController>();
        timerController = FindAnyObjectByType<TimerController>();
    }
    private void OnEnable()
    {
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        for (int i = 0; i < endGUI.transform.childCount; i++)
        {
            if (endGUI.transform.GetChild(i).gameObject.name == "Gold Time Text")
            {
                if (levelEnder.nextLevelIndex == 0)
                {
                    endGUI.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = $"Gold: {levelProgressTracker.levels[levelProgressTracker.levels.Length-1].milestone3.ToString("0:00.00")}";
                }
                else
                {
                    endGUI.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = $"Gold: {levelProgressTracker.levels[levelEnder.nextLevelIndex - 3].milestone3.ToString("0:00.00")}";
                    Debug.Log($"Index issue testing: {levelEnder.nextLevelIndex - 3}");
                }
            }
            else if (endGUI.transform.GetChild(i).gameObject.name == "Silver Time Text")
            {
                if (levelEnder.nextLevelIndex == 0)
                {
                    endGUI.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = $"Silver: {levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].milestone2.ToString("0:00.00")}";
                }
                else
                {
                    endGUI.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = $"Silver: {levelProgressTracker.levels[levelEnder.nextLevelIndex - 3].milestone2.ToString("0:00.00")}";
                }
            }
            else if (endGUI.transform.GetChild(i).gameObject.name == "Bronze Time Text")
            {
                if (levelEnder.nextLevelIndex == 0)
                {
                    endGUI.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = $"Bronze: {levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].milestone1.ToString("0:00.00")}";
                }
                else
                {
                    endGUI.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = $"Bronze: {levelProgressTracker.levels[levelEnder.nextLevelIndex - 3].milestone1.ToString("0:00.00")}";
                }
            }
        }
        

    }
    public void OnNextPress(LevelEnder lE) // player presses next after successfully completing level -sawyer
    {
        timerController.end = false;
        Time.timeScale = 1;
        if (timerController.end == false)
        {
            SceneManager.LoadScene(lE.GetNextIndex());
        }
        
    }
    public void OnRestartPress(LevelEnder lE) // player restarts -sawyer
    {
        Time.timeScale = 1;
        if (levelEnder.nextLevelIndex == 0)
        {
            Debug.Log("Git gud");
            timerController.end = false;
            if (timerController.end == false)
            {
                SceneManager.LoadScene(levelProgressTracker.levels.Length + 1);
            }
          
        }
        else
        {
            Debug.Log("Git gud");
            timerController.end = false;
            if (timerController.end == false)
            {
                SceneManager.LoadScene(lE.GetNextIndex() - 1);
            }
            
        }

    }
    public void OnQuitPress() // player likely ragequit, shame on them -sawyer
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }
}
