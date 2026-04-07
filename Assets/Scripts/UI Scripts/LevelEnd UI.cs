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
    private PauseMenu pauseMenu;
    private LevelEnder levelEnder;
    private QuickRestart quickRestart;

    [SerializeField] TextMeshProUGUI resultsDisplay;
    [SerializeField] GameObject endGUI;
    [SerializeField] GameObject planningController;
    [SerializeField] public TextMeshProUGUI bestTime;
    [SerializeField] public TextMeshProUGUI bestTimeText;
    [SerializeField] public TextMeshProUGUI thisTime;
    [SerializeField] public TextMeshProUGUI thisTimeText;
    [SerializeField] public TextMeshProUGUI timeDifference;
    [SerializeField] public TextMeshProUGUI timeDifferenceText;
    private MusicClass musicClass; 

    private void Awake()
    {
        if (quickRestart  == FindAnyObjectByType<QuickRestart>())
        {
            quickRestart = FindAnyObjectByType<QuickRestart>();
        }
        levelEnder = FindAnyObjectByType<LevelEnder>();
        if (levelEnder == null)
            Debug.Log("Level End is missing");
        else
            Debug.Log("Level End found");

        endGUI.SetActive(false);
        //timerController = GetComponent<TimerController>();
        if (FindAnyObjectByType<TimerController>())
        {
            timerController = FindAnyObjectByType<TimerController>();
        }
        if (FindAnyObjectByType<PauseMenu>())
        {
            pauseMenu = FindAnyObjectByType<PauseMenu>();
        }
        musicClass = FindAnyObjectByType<MusicClass>();
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
        lE = levelEnder;
        Time.timeScale = 1;
        if (timerController.end == false)
        {
            SceneManager.LoadScene(lE.GetNextIndex());
            levelProgressTracker.levelLoaded = true;
        }
    }
    public void OnRestartPress(LevelEnder lE) // player restarts -sawyer
    {
        Time.timeScale = 1;
        lE = levelEnder;
        if (levelEnder.nextLevelIndex == 0)
        {
            timerController.end = false;
            if (timerController.end == false)
            {
                SceneManager.LoadScene(levelProgressTracker.levels.Length + 1);
            }
        }
        else
        {
            timerController.end = false;
            if (timerController.end == false)
            {
                
                SceneManager.LoadScene(lE.GetNextIndex() - 1);
            }
        }
    }
    public void OnPlanningPress(LevelEnder lE) // player plans -sawyer
    {
        Time.timeScale = 1;
            Instantiate(planningController);
        if (levelEnder.nextLevelIndex == 0)
        {
            timerController.end = false;
            if (timerController.end == false)
            {
                SceneManager.LoadScene(levelProgressTracker.levels.Length + 1);
            }
        }
        else
        {
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
        musicClass.StopMusic();
        SceneManager.LoadScene(0);
    }
}
