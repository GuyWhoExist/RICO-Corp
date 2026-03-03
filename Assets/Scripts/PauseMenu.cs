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
    [SerializeField] PauseMenuController pauseController;
    [SerializeField] GameObject pauseHud;
    [SerializeField] GameObject gameHud;
    [SerializeField] GameObject planningGUI;
    [SerializeField] Slider FOVSlider;
    [SerializeField] TextMeshProUGUI FOVDisplay;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TextMeshProUGUI sensitivityDisplay;
    [SerializeField] GameObject configObject;
    private float sensitivityDisplayValue;
    private PlayerCamera cameraSetting;
    public bool buttonPress;
    private Config_Internal config;
    //allows to unpause via other means
 //coded by sawyer

    private void Awake()
    {
        controls = new Controls();
        Time.timeScale = 1;
        if(FindAnyObjectByType<PlanningModeController>() == false)
            planningGUI.SetActive(false);
        if (FindAnyObjectByType<PlayerCamera>())
        {
            cameraSetting = FindAnyObjectByType<PlayerCamera>();
        }

    }
    private void OnEnable()
    {
        pauseHud.SetActive(false);
        controls.Pause.Pause.Enable();
        paused = false;
        controls.Pause.Pause.performed += Pause_Performed;
        //controls.Pause.Pause.performed += (ctx) => Debug.Log("man");
        if (FindAnyObjectByType <Config_Internal>() != null) 
         {
            //Debug.Log("config exists.");
            config = FindAnyObjectByType<Config_Internal>();
            if (config.sensitivity != cameraSetting.sensitivity || config.sensitivity < 0.1 || config.sensitivity > 5.3)
            {
                cameraSetting.sensitivity = config.sensitivity;
                sensitivitySlider.value = config.sensitivity;
            }
            if (config.fieldOfView != cameraSetting.storedFOV || config.fieldOfView < 1 || config.fieldOfView > 140)
            {
                cameraSetting.storedFOV = config.fieldOfView;
                FOVSlider.value = cameraSetting.storedFOV;
            }
         }
        else
        {
            //Debug.Log("uh oh.");
        }
       
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
        buttonPress = true;

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
    private void Update()
    {
        if (cameraSetting.storedFOV != FOVSlider.value + 90)
        {
            cameraSetting.storedFOV = FOVSlider.value + 90f;
            cameraSetting.FOV = cameraSetting.storedFOV;
            config.fieldOfView = FOVSlider.value;
        }
        if (cameraSetting.sensitivity != sensitivitySlider.value + 0.3f)
        {
            cameraSetting.sensitivity = sensitivitySlider.value + 0.3f;
            config.sensitivity = sensitivitySlider.value;
        }

        sensitivityDisplayValue = Mathf.Round((sensitivitySlider.value + 0.3f) * 10);
        FOVDisplay.text = $"{FOVSlider.value + 90}";
        sensitivityDisplay.text = $"{sensitivityDisplayValue}";

        if (buttonPress == true)
        {
            if (paused == false)
            {
                Time.timeScale = 0;
                paused = true;
                gameHud.SetActive(false);
                pauseHud.SetActive(true);
                if (FindAnyObjectByType<PlanningModeController>())
                    planningGUI.SetActive(false);
                buttonPress = false;

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
               buttonPress = false;


            }
        }
    }
}
