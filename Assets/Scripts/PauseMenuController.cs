using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    //Sawyer made this one

    public PauseMenu pauseMenu;// so many public classes
    [SerializeField] GameObject pauseHUD;
    public GameObject settings_Audio;
    public GameObject settings_Video;
    public GameObject settings_Gameplay;
    public GameObject pauseUI;
    public GameObject planningController;
    public bool quit;
    private QuickRestart restartController;
    //coded by sawyer
    private void Start()
    {
        settings_Audio.SetActive(false);
        settings_Gameplay.SetActive(false);
        settings_Video.SetActive(false);

        quit = false;
    }
    public void OnPauseQuit() 
    {
        Debug.Log("You pressed it");
        pauseMenu.buttonPress = true;
        quit = true;
        if (FindAnyObjectByType<PlanningModeController>())
            Destroy(FindAnyObjectByType<PlanningModeController>().gameObject);
        SceneManager.LoadScene(0);
        
    }

    public void OnPlanningEnable()
    {
      if (FindAnyObjectByType <PlanningModeController>())
        {
            Destroy(FindAnyObjectByType<PlanningModeController>().gameObject);
            restartController = FindFirstObjectByType<QuickRestart>();
            restartController.playerDie = true;
        }
        else
        {
            Instantiate(planningController);
            restartController = FindFirstObjectByType<QuickRestart>();
            restartController.playerDie = true;
        }

    }

    public void OnSettingsOpen()
    {
        settings_Audio.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void OnTheQuintessentialResumeButtonTrigger()
    {
        pauseMenu.buttonPress = true;

    }
    public void OnAudioPress()
    {
        settings_Audio.SetActive(true);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(false);
    }
    public void OnVideoPress()
    {
        settings_Video.SetActive(true);
        settings_Gameplay.SetActive(false);
        settings_Audio.SetActive(false);
    }
    public void OnGameplayPress()
    {
        settings_Audio.SetActive(false) ;
        settings_Video.SetActive(false) ;
        settings_Gameplay.SetActive(true);
    }
    public void Update()
    {
        
    }
}
