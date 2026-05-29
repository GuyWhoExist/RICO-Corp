using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Controls controls;
    public bool paused;
    [SerializeField] GameObject PlanningModeToggle;
    [SerializeField] PauseMenuController pauseController;
    [SerializeField] GameObject pauseHud;
    [SerializeField] GameObject gameHud;
    [SerializeField] GameObject planningGUI;
    [SerializeField] Slider FOVSlider;
    [SerializeField] TMP_InputField FOVDisplay;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TMP_InputField sensitivityDisplay;
    [SerializeField] GameObject configObject;
    [SerializeField] float timeToSettingsCheck;
    private float timeForTimeToSettingsCheck;
    private LevelProgressTracker levelProgressTracker;
    private SettingsTracker settingsTracker;
    private float sensitivityDisplayValue;
    private PlayerCamera cameraSetting;
    //public bool buttonPress;
    
    //allows to unpause via other means
 //coded by sawyer

    private void Awake()
    {
        controls = new Controls();
        timeForTimeToSettingsCheck = timeToSettingsCheck;
        Time.timeScale = 1;
        planningGUI.SetActive(false);
        cameraSetting = FindAnyObjectByType<PlayerCamera>();
        levelProgressTracker = FindFirstObjectByType<LevelProgressTracker>();
        settingsTracker = FindFirstObjectByType<SettingsTracker>();

        //settingsTracker.settings.sensitivity != cameraSetting.sensitivity || 
        /*
         * if (settingsTracker.settings.sensitivity < 0.1 || settingsTracker.settings.sensitivity > 5.3)
        {
            cameraSetting.sensitivity = settingsTracker.settings.sensitivity;
            sensitivitySlider.value = settingsTracker.settings.sensitivity;
        }
        if (settingsTracker.settings.fieldOfView != cameraSetting.storedFOV || settingsTracker.settings.fieldOfView < 1 || settingsTracker.settings.fieldOfView > 140)
        {
            settingsTracker.settings.fieldOfView = cameraSetting.storedFOV;
            FOVSlider.value = cameraSetting.storedFOV;
        }
         */
        if (settingsTracker.settings.sensitivity < 0.1 || settingsTracker.settings.sensitivity > 5.3)
        {
            settingsTracker.settings.sensitivity = cameraSetting.sensitivity;
        }
        else
        {
            cameraSetting.sensitivity = settingsTracker.settings.sensitivity;
        }

        sensitivitySlider.value = settingsTracker.settings.sensitivity;

        if (settingsTracker.settings.fieldOfView < 1 || settingsTracker.settings.fieldOfView > 140)
        {
            settingsTracker.settings.fieldOfView = cameraSetting.storedFOV;
        }
        else
        {
            cameraSetting.storedFOV = settingsTracker.settings.fieldOfView;
        }

        FOVSlider.value = cameraSetting.storedFOV;

        FOVSlider.value = float.Parse(FOVDisplay.text);
        Invoke(nameof(StartControlsForPause), 0.6f);
    }
    private void OnEnable()
    {
        pauseHud.SetActive(false);

        //controls.Pause.Pause.performed += (ctx) => Debug.Log("man");
        paused = false;

        if (FindAnyObjectByType<PlanningModeController>())
        {
            gameHud.SetActive(false);
            planningGUI.SetActive(true);
        }

    }

    private void StartControlsForPause()
    {
        controls.Pause.Pause.Enable();
        controls.Pause.Pause.performed += Pause_Performed;
    }

    private void OnDisable()
    {
        controls.Pause.Pause.Disable();
    }

    private void Pause_Performed(InputAction.CallbackContext context)
    {
        ButtonPress();
        pauseController.ButtonSFX();
        if (paused == true)
        {
            pauseController.pauseUI.SetActive(true);
            if (pauseController.pauseUI == enabled)
            {
                pauseController.settings_Audio.SetActive(false);
                pauseController.settings_Video.SetActive(false);
                pauseController.settings_Gameplay.SetActive(false);
            }
        }
    }

   

    public void CompletionCheck()
    {
        if (levelProgressTracker.levelCompleted == false)
        {
            PlanningModeToggle.SetActive(false);
            Debug.Log("disabled Plans");

        }
        else
        {
            PlanningModeToggle.SetActive(true);
            Debug.Log("enabled Plans");
        }
        Debug.Log("completionCheck Fired");
    }
    public void ButtonPress()
    {
        if (paused == false && Time.timeScale != 0)
        {
            cameraSetting.Freeze();
            paused = true;
            gameHud.SetActive(false);
            pauseHud.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            if (FindAnyObjectByType<PlanningModeController>())
                planningGUI.SetActive(false);
        }
        else if (paused == true)
        {
            cameraSetting.Unfreeze();
            paused = false;
            pauseHud.SetActive(false);
            pauseController.settings_Audio.SetActive(false);
            pauseController.settings_Gameplay.SetActive(false);
            pauseController.settings_Video.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            if (FindAnyObjectByType<PlanningModeController>() == false)
                gameHud.SetActive(true);
            else
                planningGUI.SetActive(true);
        }
    }
    private void SettingsCheck()
    {
        if (cameraSetting.storedFOV != FOVSlider.value)
        {
            cameraSetting.storedFOV = FOVSlider.value;
            cameraSetting.FOV = cameraSetting.storedFOV;
            settingsTracker.settings.fieldOfView = FOVSlider.value;
        }

        if (cameraSetting.sensitivity != sensitivitySlider.value)
        {
            cameraSetting.sensitivity = sensitivitySlider.value;
            settingsTracker.settings.sensitivity = sensitivitySlider.value;
        }


        if (float.Parse(FOVDisplay.text) > FOVSlider.maxValue)
        {
            FOVDisplay.text = FOVSlider.maxValue.ToString();
        }

        if (float.Parse(sensitivityDisplay.text) > sensitivitySlider.maxValue)
        {
            sensitivityDisplay.text = sensitivitySlider.maxValue.ToString();
        }

        settingsTracker.settings.sensitivity = cameraSetting.sensitivity;
        settingsTracker.settings.fieldOfView = cameraSetting.FOV;
    }


    public void OnFOVSliderChange()
    {
        FOVDisplay.text = FOVSlider.value.ToString();
    }

    public void OnFOVInputChange()
    {
        FOVSlider.value = float.Parse(FOVDisplay.text);
    }

    public void OnSensSliderChange()
    {
        sensitivityDisplayValue = Mathf.Round((sensitivitySlider.value + 0.3f) * 10);


        sensitivityDisplay.text = $"{sensitivityDisplayValue}";
    }

    public void OnSensInputChange()
    {
        sensitivityDisplayValue = MathF.Round(float.Parse(sensitivityDisplay.text + 0.3f) * 10);
    }


    private void Update()
    {
        timeToSettingsCheck -= Time.unscaledDeltaTime;
        if (timeToSettingsCheck < 0)
        {
            SettingsCheck();
            timeToSettingsCheck = timeForTimeToSettingsCheck;
        }
    }
}
