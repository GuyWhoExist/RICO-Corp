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
       // settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnRestartPressRun), 0.1f);
    }
    private void OnRestartPressRun()
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
      //  settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnFullQuitRun), 0.1f);
    }
    private void OnFullQuitRun()
    {
        tC.SaveSprays();
        Application.Quit();
        //Debug.Log("You closed it");
    }
    public void OnPauseQuit() 
    {
      //  settingsAudio.PlayOneShot(buttonPress, 0.1f);
        //Debug.Log("You pressed it");
        Invoke(nameof(OnPauseQuitRun), 0.1f);  
    }
    private void OnPauseQuitRun()
    {
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
      //  settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnPlanningEnableRun), 0.1f);
    }
    private void OnPlanningEnableRun()
    {
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
      //  settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnSettingsOpenRun), 0.1f);
    }
    private void OnSettingsOpenRun()
    {
        settings_Audio.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void OnTheQuintessentialResumeButtonTrigger()
    {
      //  settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnResumeRun), 0.1f);
    }
    private void OnResumeRun()
    {
        pauseMenu.ButtonPress();
    }
    public void OnAudioPress()
    {
      //  settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnAudioRun), 0.1f);
    }
    private void OnAudioRun()
    {
        settings_Audio.SetActive(true);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(false);
    }
    public void OnVideoPress()
    {
      //  settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnVideoRun), 0.1f);
    }
    private void OnVideoRun()
    {
        settings_Audio.SetActive(false);
        settings_Video.SetActive(true);
        settings_Gameplay.SetActive(false);
    }
    public void OnGameplayPress()
    {
       // settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnGameplayRun), 0.1f);
    }
    private void OnGameplayRun()
    {
        settings_Audio.SetActive(false);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(true);
    }

    public void OnBackPress()
    {
       // settingsAudio.PlayOneShot(buttonPress, 0.1f);
        Invoke(nameof(OnBackRun), 0.1f);
    }
    private void OnBackRun()
    {
        settings_Audio.SetActive(false);
        settings_Video.SetActive(false);
        settings_Gameplay.SetActive(false);
        pauseUI.SetActive(true);
    }

}
