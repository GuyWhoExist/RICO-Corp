using System;
using NUnit.Framework.Internal;
using Palmmedia.ReportGenerator.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
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
    private float sensitivityDisplayValue;
    private PlayerCamera cameraSetting;
    //public bool buttonPress;
    private Config_Internal config;
    
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
        
        config = FindFirstObjectByType<Config_Internal>();
      

            if (config.sensitivity != cameraSetting.sensitivity || config.sensitivity < 0.1 || config.sensitivity > 5.3)
            {
                cameraSetting.sensitivity = config.sensitivity;
                sensitivitySlider.value = config.sensitivity;
            }
            if (config.fieldOfView != cameraSetting.storedFOV || config.fieldOfView < 1 || config.fieldOfView > 140)
            {
                config.fieldOfView = cameraSetting.storedFOV;
                FOVSlider.value = cameraSetting.storedFOV;
            }

        FOVSlider.value = float.Parse(FOVDisplay.text);
        
    }
    private void OnEnable()
    {
        pauseHud.SetActive(false);
        controls.Pause.Pause.Enable();
        paused = false;
        controls.Pause.Pause.performed += Pause_Performed;
        //controls.Pause.Pause.performed += (ctx) => Debug.Log("man");
       
       
        if (FindAnyObjectByType<PlanningModeController>())
        {
            gameHud.SetActive(false);
            planningGUI.SetActive(true);
        }
            

    }
    private void OnDisable()
    {
        controls.Pause.Pause.Disable();
    }

    private void Pause_Performed(InputAction.CallbackContext context)
    {
        ButtonPress();

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
        if (paused == false)
        {
            Time.timeScale = 0;
            paused = true;
            gameHud.SetActive(false);
            pauseHud.SetActive(true);
            if (FindAnyObjectByType<PlanningModeController>())
                planningGUI.SetActive(false);
        }
        else if (paused == true)
        {
            Time.timeScale = 1;
            paused = false;
            pauseHud.SetActive(false);

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
            config.fieldOfView = FOVSlider.value;
        }

        if (cameraSetting.sensitivity != sensitivitySlider.value + 0.3f)
        {
            cameraSetting.sensitivity = sensitivitySlider.value + 0.3f;
            config.sensitivity = sensitivitySlider.value;
        }


        if (float.Parse(FOVDisplay.text) > FOVSlider.maxValue)
        {
            FOVDisplay.text = FOVSlider.maxValue.ToString();
        }

        if (float.Parse(sensitivityDisplay.text) > sensitivitySlider.maxValue)
        {
            sensitivityDisplay.text = sensitivitySlider.maxValue.ToString();
        }
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
