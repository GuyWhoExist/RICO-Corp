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
    private MusicClass musicClass;
    [SerializeField] private TimerController tC;

    //coded by sawyer
    private void Awake()
    {
        musicClass = FindFirstObjectByType<MusicClass>();
    }

    private void Start()
    {
        settings_Audio.SetActive(false);
        settings_Gameplay.SetActive(false);
        settings_Video.SetActive(false);
        if (FindFirstObjectByType<QuickRestart>() )
        {
            restartController = FindFirstObjectByType<QuickRestart>();
        }
        quit = false;
       
    }
    public void OnRestartPress()
    {
        if (restartController != null)
        {
            tC.SaveSprays();
            pauseMenu.ButtonPress();
            restartController.PlayerDie();
        }
    }
    public void OnFullQuit()
    {
        tC.SaveSprays();
        Application.Quit();
        //Debug.Log("You closed it");
    }
    public void OnPauseQuit() 
    {
        //Debug.Log("You pressed it");
        tC.SaveSprays();
        pauseMenu.ButtonPress();
        quit = true;
        musicClass.StopMusic();
        if (FindAnyObjectByType<PlanningModeController>())
            Destroy(FindAnyObjectByType<PlanningModeController>().gameObject);
        SceneManager.LoadScene(0);
        
    }

    public void OnPlanningEnable()
    {
      if (FindAnyObjectByType <PlanningModeController>() != null)
        {
            tC.SaveSprays();
            Destroy(FindAnyObjectByType<PlanningModeController>().gameObject);
            restartController = FindFirstObjectByType<QuickRestart>();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Instantiate(planningController);
            restartController = FindFirstObjectByType<QuickRestart>();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    public void OnSettingsOpen()
    {
        settings_Audio.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void OnTheQuintessentialResumeButtonTrigger()
    {
        pauseMenu.ButtonPress();
    }
    public void OnAudioPress()
    {
        settings_Audio.SetActive(true);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(false);
    }
    public void OnVideoPress()
    {
        settings_Audio.SetActive(false);
        settings_Video.SetActive(true);
        settings_Gameplay.SetActive(false);
    }
    public void OnGameplayPress()
    {
        settings_Audio.SetActive(false) ;
        settings_Video.SetActive(false) ;
        settings_Gameplay.SetActive(true);
    }

    public void OnBackPress()
    {
        settings_Audio.SetActive(false);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(false);
        pauseUI.SetActive(true);
    }
}
