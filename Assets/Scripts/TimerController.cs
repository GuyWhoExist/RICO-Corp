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
    public float curTime;
    public bool timeTicking;
    public bool end;

    private void Awake()
    {
        if (FindAnyObjectByType<PlanningModeController>() == null)
        {
            timeTicking = true;
        }
        else
        {
            timeTicking = false;
        }
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        if (levelProgressTracker != null )
        {
            Debug.Log("We good in the time controller");
        }
        else
        {
            Debug.Log("Things have gone HORRIBLY wrong in the time controller");
        }
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


    void Update()
    {
        if (timeTicking)
        {
            curTime += Time.deltaTime;
            timerText.text = curTime.ToString("0:00.00");
        }
        if (curTime >= 60f) //this changes the value of curTime to follow the 0:00.0 format - Nova
        {
            float tempTime = curTime;
            while (tempTime - 100f > 0)
            {
                tempTime -= 100f;
            }
            if (tempTime >= 60)
            {
                curTime = 100f + (curTime - 60);
            }
        }
        Enemy[] enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        enemyCountText.text = $"Enemies Left: {enemyNumber.Length}";
        enemyCountText2.text = $"Enemies Left: {enemyNumber.Length}";


        //for (int i = 0; i < levelProgressTracker.levels.Length; i++)
        //{
        //    testArray[i] = levelProgressTracker.levels[i].bestTime;
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<LevelEnder>() != null && FindAnyObjectByType<PlanningModeController>() == null)
        {
            levelProgressTracker.used = true;
            LevelEnder lE = collision.transform.GetComponent<LevelEnder>();
            if (timeTicking)
            {
                Enemy[] enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                timeTicking = false;
                if (enemyNumber.Length == 0)
                {
                    curTime -= lE.GetBonus();
                    timerText.text = curTime.ToString("0:00.00");
                }
                if (lE.GetNextIndex() == 0)
                {
                    if (curTime < levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].bestTime || levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].bestTime == -1f)
                    {
                        levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].bestTime = curTime;
                    }
                }
                else
                {
                    if (curTime < levelProgressTracker.levels[lE.GetNextIndex() - 3].bestTime || levelProgressTracker.levels[lE.GetNextIndex() - 3].bestTime == -1f)
                    {
                        levelProgressTracker.levels[lE.GetNextIndex() - 3].bestTime = curTime;
                    }
                }
                Debug.Log("best time updated (hopefully)");
                StartCoroutine(WaitABit(lE));
            }
        }
    }
  

    private IEnumerator WaitABit(LevelEnder lE)
    {
        yield return new WaitForSeconds(1f);
        MusicClass test = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>();
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



        if (lE.GetNextIndex() == 0)
        {
            if (curTime <= levelProgressTracker.levels[levelProgressTracker.levels.Length - 1].milestone1) //if the player beat milestone 1 we load next level - Nova
            {
                Debug.Log("Milestone 1 hit");
                SceneManager.LoadScene(lE.GetNextIndex());
            }
            else //otherwise, load the scene again
            {
                Debug.Log("Git gud");
                SceneManager.LoadScene(levelProgressTracker.levels.Length + 1);
            }
        }
        else
        {

            endGUI.SetActive(true);
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

        }
    }
}
