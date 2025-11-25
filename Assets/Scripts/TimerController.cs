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
    [SerializeField] GameObject next;
    [SerializeField] GameObject endGUI;
    [SerializeField] GameObject gameHUD;
    private LevelProgressTracker levelProgressTracker;
    public float curTime;
    public bool timeTicking;
    public bool end;

    private void Awake()
    {
        timeTicking = true;
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        if (levelProgressTracker != null )
        {
            Debug.Log("We good in the time controller");
        }
        else
        {
            Debug.Log("Things have gone HORRIBLY wrong in the time controller");
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
        if (curTime >= 60f)
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
        enemyCountText.text = ($"Enemies Left: {enemyNumber.Length}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<LevelEnder>() != null)
        {
            LevelEnder lE = collision.transform.GetComponent<LevelEnder>();
            if (timeTicking)
            {
                Enemy[] enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                if (enemyNumber.Length == 0)
                {
                    curTime -= lE.GetBonus();
                    timeTicking = false;
                    timerText.text = curTime.ToString("0:00.00");
                }
                else
                {
                    curTime += 2.5f * enemyNumber.Length;
                    timeTicking = false;
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
                levelProgressTracker.used = true;
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

            if (curTime <= levelProgressTracker.levels[lE.GetNextIndex() - 2].milestone1)
            {

                next.SetActive(true);
                endGUI.SetActive(true);
                Time.timeScale = 0;
                gameHUD.SetActive(false);
                end = true;
            }
            else
            {
                next.SetActive(false);
                endGUI.SetActive(true);
                Time.timeScale = 0;
                gameHUD.SetActive(false);
                end = true;
            }


            levelProgressTracker.used = true;

        }
    }
}
