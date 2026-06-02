using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.SceneView;

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
    public LevelProgressTracker levelProgressTracker;
    private SettingsTracker settingsTracker;
    private float sensitivityDisplayValue;
    private PlayerCamera cameraSetting;
    //public bool buttonPress;
    
    //allows to unpause via other means
 //coded by sawyer

    private void Awake()
    {
        controls = new Controls();
        //timeForTimeToSettingsCheck = timeToSettingsCheck;
        
        Time.timeScale = 1;
        planningGUI.SetActive(false);
        cameraSetting = FindAnyObjectByType<PlayerCamera>();
        if (cameraSetting != null )
        {
            Debug.Log("cam Set good in Pause Menu");
        }
        else
        {
            Debug.Log("cam Set missing in Pause Menu");
        }

        levelProgressTracker = FindFirstObjectByType<LevelProgressTracker>();
        if (levelProgressTracker != null)
        {
            Debug.Log("LPT good in Pause Menu");
        }
        else
        {
            Debug.Log("LPT missing in Pause Menu");
        }

        settingsTracker = FindFirstObjectByType<SettingsTracker>();
        if (settingsTracker != null)
        {
            Debug.Log("set Track good in Pause Menu");
        }
        else
        {
            Debug.Log("set Track missing in Pause Menu");
        }
        StartControlsForPause();
        LoadSettings();
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
         
        if (settingsTracker.settings.sensitivity < 0.1 || settingsTracker.settings.sensitivity > 5.3)
        {
            settingsTracker.settings.sensitivity = cameraSetting.sensitivity;
        }
        else
        {
            cameraSetting.sensitivity = settingsTracker.settings.sensitivity;
        }

        sensitivitySlider.value = settingsTracker.settings.sensitivity;

        if (settingsTracker.settings.fieldOfView < 1 || settingsTracker.settings.fieldOfView > 150)
        {
            settingsTracker.settings.fieldOfView = cameraSetting.storedFOV;
        }
        else
        {
            cameraSetting.storedFOV = settingsTracker.settings.fieldOfView;
        }

        FOVSlider.value = cameraSetting.storedFOV;

        Invoke(nameof(StartControlsForPause), 0.6f);
        */
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
        Debug.Log($"Pause pressed, state on press: {paused}");
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
            SaveSettings();
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
        cameraSetting.FOV = FOVSlider.value;
        FOVDisplay.text = FOVSlider.value.ToString();
        UpdateFOV();
    }

    public void OnFOVInputChange()
    {
        if (float.TryParse(FOVDisplay.text, out float result))
        {
            if (result < 10)
            {
                FOVSlider.value = 10f;
                FOVDisplay.text = "10";
            }
            else if (result > 150f)
            {
                FOVSlider.value = 150f;
                FOVDisplay.text = "150";
            }
            else
            {
                FOVSlider.value = result;
                FOVDisplay.text = result.ToString();
            }
            UpdateFOV();
        }
    }

    public void OnSensSliderChange()
    {
        sensitivityDisplayValue = Mathf.Round((sensitivitySlider.value) * 10);
        sensitivityDisplay.text = $"{sensitivityDisplayValue}";
        Debug.Log($"sens is now: {sensitivitySlider.value}");
        cameraSetting.sensitivity = sensitivitySlider.value;
    }

    public void OnSensInputChange()
    {
        if (float.TryParse(sensitivityDisplay.text, out float result))
        {
            sensitivityDisplayValue = result;
            sensitivityDisplay.text = result.ToString();
            sensitivitySlider.value = result / 10;
            cameraSetting.sensitivity = sensitivitySlider.value;
        } 
    }

    private void LoadSettings()
    {
        Debug.Log($"Changing local sens ({cameraSetting.sensitivity}) to saved sens ({settingsTracker.settings.sensitivity})");
        sensitivitySlider.value = settingsTracker.settings.sensitivity;
        cameraSetting.sensitivity = sensitivitySlider.value;
        Debug.Log($"Changing local FOV ({cameraSetting.FOV}) to saved FOV ({settingsTracker.settings.fieldOfView})");
        cameraSetting.FOV = settingsTracker.settings.fieldOfView;
        Debug.Log($"Changing local stored(?) FOV ({cameraSetting.storedFOV}) to saved FOV ({settingsTracker.settings.fieldOfView})");
        cameraSetting.storedFOV = settingsTracker.settings.fieldOfView;
        FOVSlider.value = settingsTracker.settings.fieldOfView;
        FOVDisplay.text = FOVSlider.value.ToString();
        Debug.Log($"Changing sens display ({sensitivityDisplayValue}) to saved sens ({settingsTracker.settings.sensitivity})");
        sensitivityDisplayValue = Mathf.Round((sensitivitySlider.value) * 10);
        sensitivityDisplay.text = sensitivityDisplayValue.ToString();
    }

    public void SaveSettings()
    {
        Debug.Log($"Changing saved sens ({settingsTracker.settings.sensitivity}) to local sens ({cameraSetting.sensitivity})");
        settingsTracker.settings.sensitivity = cameraSetting.sensitivity;
        Debug.Log($"Changing saved FOV ({settingsTracker.settings.fieldOfView}) to local FOV ({cameraSetting.FOV})");
        settingsTracker.settings.fieldOfView = cameraSetting.FOV;
    }

    private void UpdateFOV()
    {
        Debug.Log($"Tried updating FOV from {cameraSetting.playerCamera.fieldOfView} to {cameraSetting.FOV}");
        cameraSetting.playerCamera.fieldOfView = cameraSetting.FOV;
        cameraSetting.storedFOV = cameraSetting.FOV;
    }

    /*
    private void Update()
    {
        timeToSettingsCheck -= Time.unscaledDeltaTime;
        if (timeToSettingsCheck < 0)
        {
            SettingsCheck();
            timeToSettingsCheck = timeForTimeToSettingsCheck;
        }
    }
    */
}
