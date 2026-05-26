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
        StartCoroutine(OnResumeRun());
    }
    private IEnumerator OnRestartPressRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            if (restartController != null)
            {
                tC.SaveSprays();
                pauseMenu.ButtonPress();
                restartController.PlayerDie();
            }
            StopCoroutine(OnRestartPressRun());
            inputDelay = 0;
            yield return null;
        }
      
    }
    public void OnFullQuit()
    {
        ButtonSFX();
        StartCoroutine(OnFullQuitRun());
    }
    private IEnumerator OnFullQuitRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            tC.SaveSprays();
            Application.Quit();
            yield return null;
        }
       
        //Debug.Log("You closed it");
    }
    public void OnPauseQuit() 
    {
        ButtonSFX();
        //Debug.Log("You pressed it");
       StartCoroutine(OnPauseQuitRun());
    }
    private IEnumerator OnPauseQuitRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            tC.SaveSprays();
            pauseMenu.ButtonPress();
            quit = true;
            musicClass.StopMusic();
            if (FindAnyObjectByType<PlanningModeController>())
                Destroy(FindAnyObjectByType<PlanningModeController>().gameObject);
            SceneManager.LoadScene(0);
            StopCoroutine (OnPauseQuitRun());
            inputDelay = 0;
            yield return null;
        }
      
    }
    public void OnPlanningEnable()
    {
        ButtonSFX();
        StartCoroutine(OnPlanningEnableRun());
    }
    private IEnumerator OnPlanningEnableRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
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
            StopCoroutine(OnPlanningEnableRun());
            inputDelay = 0;
            yield return null;
        }
       
    }
    public void OnSettingsOpen()
    {
        ButtonSFX();
        StartCoroutine(OnSettingsOpenRun());
    }
    private IEnumerator OnSettingsOpenRun()
    {
        
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            settings_Audio.SetActive(true);
            pauseUI.SetActive(false);
            StopCoroutine(OnSettingsOpenRun());
            inputDelay = 0;
            yield return null;
        }
      
    }
    public void OnTheQuintessentialResumeButtonTrigger()
    {
        ButtonSFX();
        StartCoroutine(OnResumeRun());
    }
    private IEnumerator OnResumeRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            pauseMenu.ButtonPress();
            StopCoroutine(OnResumeRun());
            inputDelay = 0;
            yield return null;
        }
        
    }
    public void OnAudioPress()
    {
        ButtonSFX();
        StartCoroutine(OnAudioRun());
    }
    private IEnumerator OnAudioRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            settings_Audio.SetActive(true);
            settings_Video.SetActive(false);
            settings_Gameplay.SetActive(false);
            StopCoroutine (OnAudioRun());
            inputDelay = 0;
            yield return null;
        }
       
    }
    public void OnVideoPress()
    {
        ButtonSFX();
        StartCoroutine(OnVideoRun());
    }
    private IEnumerator OnVideoRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            settings_Audio.SetActive(false);
            settings_Video.SetActive(true);
            settings_Gameplay.SetActive(false);
            StopCoroutine (OnVideoRun());
            inputDelay = 0;
            yield return null;
        }
      
    }
    public void OnGameplayPress()
    {
        ButtonSFX();
       StartCoroutine(OnGameplayRun());
    }
    private IEnumerator OnGameplayRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            settings_Audio.SetActive(false);
            settings_Video.SetActive(false);
            settings_Gameplay.SetActive(true);
            StopCoroutine (OnGameplayRun());
            inputDelay = 0;
            yield return null;
        }
      
    }
    public void OnBackPress()
    {
        ButtonSFX();
        StartCoroutine(OnBackRun());
    }
    private IEnumerator OnBackRun()
    {
        inputDelay += Time.unscaledDeltaTime;
        while (inputDelay >= 0.1f)
        {
            settings_Audio.SetActive(false);
            settings_Video.SetActive(false);
            settings_Gameplay.SetActive(false);
            pauseUI.SetActive(true);
            StopCoroutine(OnBackRun());
            inputDelay = 0;
            yield return null;
        }
    }
    public void ButtonSFX()
    {
        settingsAudio.PlayOneShot(buttonPress);
    }
}
