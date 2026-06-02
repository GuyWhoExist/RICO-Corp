using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    private PlayerCamera pCamera;
    [HideInInspector] public bool statusCheck;
    [SerializeField] Button planningModeToggle;
    [SerializeField] TextMeshProUGUI popUp;

    public float curTime;
    public bool timeTicking;
    public bool planState = false;
    public bool end;
    private SaveSystem saveSystem;
    private LevelEndUI levelEndUI;
    public bool delete;

    private void Awake()
    {
        levelEndUI = FindAnyObjectByType<LevelEndUI>();
        saveSystem = FindAnyObjectByType<SaveSystem>();
        pCamera = FindAnyObjectByType<PlayerCamera>();

       // besttimeconversion();

        popUp.enabled = false;

        if (FindAnyObjectByType<PlanningModeController>() != null)
        {
            planState = true;
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

        levelProgressTracker.LoadSprays(SceneManager.GetActiveScene().buildIndex - 1);
    }
    void Update()
    {
        if (timeTicking && !planState)
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
        if (delete)
        {
            Debug.Log("Delete is true!!!1 WHY????");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collisiony");
        if (collision.transform.GetComponent<LevelEnder>() != null && FindAnyObjectByType<PlanningModeController>() == null)
        {
            if (pauseMenu.paused)
            {
                pauseMenu.ButtonPress();
                Debug.Log("pausemenu issues with the end screen ");
            }

            timerText.enabled = false;
            UnityEngine.Cursor.lockState = CursorLockMode.None;

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
            levelEndUI.enabled = true;

            if (timeTicking)
            {
                Enemy[] enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                timeTicking = false;

                if (enemyNumber.Length == 0)
                {
                    timerText.text = curTime.ToString("0:00.00");
                }

                if (levelProgressTracker.levels[SceneManager.GetActiveScene().buildIndex - 1].bestTime > curTime || levelProgressTracker.levels[SceneManager.GetActiveScene().buildIndex - 1].bestTime == -1f)
                {
                    levelProgressTracker.levels[SceneManager.GetActiveScene().buildIndex - 1].bestTime = curTime;
                }

                if (SceneManager.GetActiveScene().buildIndex == levelProgressTracker.levels.Length)
                {
                    musicClass.StopMusic();
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
  
    public void SaveSprays() //saving sprays - Nova
    {
        LevelEnder lE = FindAnyObjectByType<LevelEnder>();
        int location = 0; //the location/index of where we are storing the current sprays - Nova
        Spray[] foundSprays = FindObjectsByType<Spray>(FindObjectsSortMode.None);
        
        location = SceneManager.GetActiveScene().buildIndex-1;

        levelProgressTracker.sprays[location] = new List<LevelProgressTrackerDTO.SprayInfo>();

        for (int i = 0; i < FindObjectsByType<Spray>(FindObjectsSortMode.None).Length; i++) 
        {
            if (foundSprays[i].prePlaced == false) //checks only for sprays not spawned from loading - Nova
            {
                levelProgressTracker.sprays[location].Add(new LevelProgressTrackerDTO.SprayInfo(foundSprays[i].Position, foundSprays[i].rotation, foundSprays[i].savedSpray, foundSprays[i].destructible));
                Debug.Log($"Spray {i} saved in {location}");
            }
            Debug.Log($"Spray spawned status: {foundSprays[i].spawned}");
            Debug.Log($"Spray prePlaced status: {foundSprays[i].prePlaced}");
            Debug.Log($"Delete status: {delete}");
        }
        
        saveSystem.DTOsave();
        delete = false;
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


        endGUI.SetActive(true);
        pCamera.Freeze();

        if (levelProgressTracker.initialComplete == true)
        {
            popUp.enabled = true;
        }

        for (int i = 0; i < gameHUD.transform.childCount; i++) //disables everything in gameHUD except the timer when the level ends - Nova
        {
            if (gameHUD.transform.GetChild(i).gameObject.name != "Timer")
            {
                gameHUD.transform.GetChild(i).gameObject.SetActive(false);

                pauseMenu.controls.Pause.Pause.Disable();
            }
        }

        end = true;

        if (lE.nextLevelIndex == 0 || curTime <= levelProgressTracker.levels[SceneManager.GetActiveScene().buildIndex - 1].milestone1)
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
