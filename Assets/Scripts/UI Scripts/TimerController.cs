using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TimerController : MonoBehaviour
{
    //controls UI, time, and changing levels for some reason idk - Nova

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI enemyCountText2;
    [SerializeField] GameObject next;
    [SerializeField] GameObject endGUI;
    [SerializeField] GameObject gameHUD;
    [SerializeField] PauseMenu pauseMenu;
    private LevelProgressTracker levelProgressTracker;
    private MusicClass musicClass;
    private LevelProgressTrackerDTO levelProgressTrackerDTO;
    [HideInInspector] public bool statusCheck;
    [SerializeField] Button planningModeToggle;
    [SerializeField] TextMeshProUGUI popUp;

    public float curTime;
    public bool timeTicking;
    public bool end;
    private SaveSystem saveSystem;
    private LevelEndUI levelEndUI;  

    private void Awake()
    {
        if (FindAnyObjectByType<LevelEndUI>())
        {
            levelEndUI = FindAnyObjectByType<LevelEndUI>();
        }
        saveSystem = FindAnyObjectByType<SaveSystem>();
        if (saveSystem != null)
        {
            //Debug.Log("we good in the time controller");
        }
        else
        {
            //Debug.Log("things have gone horribly wrong in the time controller");
        }
       // besttimeconversion();
    
            timeTicking = true;
        popUp.enabled = false;
        if (FindAnyObjectByType<PlanningModeController>() == null)
        {
            timeTicking = true;
        }
        else
        {
            timeTicking = false;
        }
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        musicClass = FindAnyObjectByType<MusicClass>();
        musicClass.PlayMusic();

       

        if (levelProgressTracker != null )
        {
            //Debug.Log("We good in the time controller");
        }
        else
        {
            //Debug.Log("Things have gone HORRIBLY wrong in the time controller");
        }

       //levelProgressTrackerDTO = FindAnyObjectByType<LevelProgressTrackerDTO>();
       // if (levelProgressTrackerDTO != null)
       // {
       //     Debug.Log("We good in the time controller");
       // }
       // else
       // {
       //     Debug.Log("Things have gone HORRIBLY wrong in the time controller");
       // }


        levelProgressTracker.used = true;
        for (int i = 0; i < gameHUD.transform.childCount; i++) //Enables everything in gameHUD except the timer when the level starts - Nova
        {
            if (gameHUD.transform.GetChild(i).gameObject.name != "Timer")
            {
                gameHUD.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        end = false;
    }

    private void Start()
    {

        if (levelProgressTracker.levelCompleted == false)
        {
        timerText.enabled = false;
        Debug.Log("disabled timer");
        statusCheck = true;
        }
        else
        {
        timerText.enabled = true;
        Debug.Log("enabled Timer");
        }
    }
    void Update()
    {
       
        if (timeTicking)
        {
            curTime += Time.deltaTime;
            timerText.text = curTime.ToString("0:00.00");
        }
        if (curTime >= 60f) //this changes the value of curTime to follow the 0:00.00 format - Nova
        {
            float tempTime = curTime;
            while (tempTime - 100f > 0) //eliminate the minutes part of the time to check seconds - Nova
            {
                tempTime -= 100f;
            }
            if (tempTime >= 60) //if the seconds part hits 60, increase the minutes and reset the seconds - Nova
            {
                curTime = 100f + (curTime - 60);
            }
        }
        if (levelProgressTracker.cheatsEnemyCountStatus)
        {
            Enemy[] enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None); //we always check the amount of enemies in the scene - Nova
            enemyCountText.text = $"Enemies Left: {enemyNumber.Length}"; //updates the enemy count for both the game UI and planning UI - Nova
            enemyCountText2.text = $"Enemies Left: {enemyNumber.Length}"; // the same but for planning mode. these are both enableable via cheats.
        }
        else
        {
            enemyCountText.enabled = false;
            enemyCountText2.enabled = false;
        }
      

        //for (int i = 0; i < levelProgressTracker.levels.Length; i++)
        //{
        //    testArray[i] = levelProgressTracker.levels[i].bestTime;
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<LevelEnder>() != null && FindAnyObjectByType<PlanningModeController>() == null)
        {
            timerText.enabled = false;
            if (levelProgressTracker.bestTimeStored > -1)
            {
                
                levelEndUI.bestTime.text = levelProgressTracker.bestTimeStored.ToString("0:00.00");
                if (levelProgressTracker.bestTimeStored > curTime)
                {
                    levelEndUI.bestTimeText.text = ("former best time");
                    levelEndUI.timeDifference.color = Color.green;
                    levelEndUI.timeDifferenceText.color = Color.green;
                    levelEndUI.timeDifference.text = Mathf.Abs(curTime - levelProgressTracker.bestTimeStored).ToString("0:00.00");
                }
                else if (levelProgressTracker.bestTimeStored < curTime)
                {
                    levelEndUI.bestTimeText.text = ("current best time");
                    levelEndUI.timeDifference.color = Color.red;
                    levelEndUI.timeDifferenceText.color = Color.red;
                    levelEndUI.timeDifference.text = Mathf.Abs(levelProgressTracker.bestTimeStored - curTime).ToString("0:00.00");
                }
            }
            else
            {
                levelEndUI.bestTime.text = ("0:00.00");
            }

            levelEndUI.thisTime.text = timerText.text;

            levelProgressTracker.used = true;
            musicClass.used = true;
            LevelEnder lE = collision.transform.GetComponent<LevelEnder>();
            if (timeTicking)
            {
                Enemy[] enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                timeTicking = false;
                if (enemyNumber.Length == 0)
                {
                    timerText.text = curTime.ToString("0:00.00");
                }
                if (lE.GetNextIndex() == 0)
                {
                    if (curTime < levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].bestTime || levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].bestTime == -1f)
                    {
                        levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].bestTime = curTime;
                    }
                    musicClass.StopMusic();
                }
                else
                {
                    if (curTime < levelProgressTracker.levels[lE.GetNextIndex() - 3].bestTime || levelProgressTracker.levels[lE.GetNextIndex() - 3].bestTime == -1f)
                    {
                        levelProgressTracker.levels[lE.GetNextIndex() - 3].bestTime = curTime;
                    }
                }
                //level ends, save best times
                Debug.Log("best time updated (hopefully)");
                StartCoroutine(WaitABit(lE));
                //run the bestTimeDTO
                //saveSystem.bestTimeConversion();
                saveSystem.DTOsave();
            }
        }
    }
  

    private IEnumerator WaitABit(LevelEnder lE)
    {
        yield return new WaitForSeconds(1f);
        /*MusicClass test = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>();
        if (test != null)
        {
            if (lE.GetNextIndex() == 0)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                Debug.Log("Music is stopped");
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
            }
            else if (lE.GetNextIndex() != 0)
            {
                Debug.Log("Music is now playing (time controller)");
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
            }
        }
        else
        {
            Debug.Log("No Audio Source Found!");
        }
        */


        if (lE.GetNextIndex() == 0)
        {
            if (curTime <= levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].milestone1) //if the player beat milestone 1 we load next level - Nova
            {
                //Debug.Log("Milestone 1 hit");
                SceneManager.LoadScene(lE.GetNextIndex());
            }
            else //otherwise, load the scene again
            {
                //Debug.Log("Git gud");
                SceneManager.LoadScene(levelProgressTracker.levels.Length + 1);
            }
        }
        else
        {

            endGUI.SetActive(true);
            if (levelProgressTracker.initialComplete == true)
            {
                popUp.enabled = true;
            }
            Time.timeScale = 0;
            for (int i = 0; i < gameHUD.transform.childCount; i++) //disables everything in gameHUD except the timer when the level ends - Nova
            {
                if (gameHUD.transform.GetChild(i).gameObject.name != "Timer")
                {
                    gameHUD.transform.GetChild(i).gameObject.SetActive(false);

                    pauseMenu.controls.Pause.Pause.Disable();
                }
            }
            end = true;
            
            if (curTime <= levelProgressTracker.levels[lE.GetNextIndex() - 3].milestone1)
            {
                next.SetActive(true);
            }
            else
            {
                next.SetActive(false);
            }
            levelProgressTracker.used = true;
            musicClass.used = true;

        }
    }
}
