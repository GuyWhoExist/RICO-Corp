using System.Collections;
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
    public AudioClip buttonPress;
    [SerializeField] AudioSource settingsAudio;
    private float inputDelay;

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
        settingsAudio.PlayOneShot(buttonPress, 0.01f);
        if (restartController != null)
        {
            tC.SaveSprays();
            pauseMenu.ButtonPress();
            restartController.PlayerDie();
        }
    }
   
    public void OnFullQuit()
    {
        ButtonSFX();
        tC.SaveSprays();
        Application.Quit();

    }
 
    public void OnPauseQuit() 
    {
        ButtonSFX();
        tC.SaveSprays();
        pauseMenu.ButtonPress();
        quit = true;
        musicClass.StopMusic();
        if (FindAnyObjectByType<PlanningModeController>())
            Destroy(FindAnyObjectByType<PlanningModeController>().gameObject);
        SceneManager.LoadScene(0);
        //Debug.Log("You pressed it");
    }
  
    public void OnPlanningEnable()
    {
        ButtonSFX();
        if (FindAnyObjectByType<PlanningModeController>() != null)
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
        ButtonSFX();
        settings_Audio.SetActive(true);
        pauseUI.SetActive(false);
    }
  
    public void OnTheQuintessentialResumeButtonTrigger()
    {
        ButtonSFX();
        pauseMenu.ButtonPress();
    }
    public void OnAudioPress()
    {
        ButtonSFX();
        settings_Audio.SetActive(true);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(false);
    }
 
    public void OnVideoPress()
    {
        ButtonSFX();
        settings_Audio.SetActive(false);
        settings_Video.SetActive(true);
        settings_Gameplay.SetActive(false);
    }
    public void OnGameplayPress()
    {
        ButtonSFX();
        settings_Audio.SetActive(false);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(true);
    }
 
    public void OnBackPress()
    {
        ButtonSFX();
        settings_Audio.SetActive(false);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(false);
        pauseUI.SetActive(true);
    }
    public void ButtonSFX()
    {
        settingsAudio.PlayOneShot(buttonPress);
    }
}
