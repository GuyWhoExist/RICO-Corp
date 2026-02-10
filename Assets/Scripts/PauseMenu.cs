using System;
using NUnit.Framework.Internal;
using Palmmedia.ReportGenerator.Core;
using TMPro;
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
    private float sensitivityDisplayValue;
    private PlayerCamera cameraSetting;
    public bool buttonPress;
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
        controls.Pause.Pause.performed += (ctx) => Debug.Log("man");
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
                if (cameraSetting.storedFOV != FOVSlider.value + 90)
                {
                    cameraSetting.storedFOV = FOVSlider.value + 90f;
                    cameraSetting.FOV = cameraSetting.storedFOV;
                }
                if (cameraSetting.sensitivity != sensitivitySlider.value + 0.3f)
                {
                    cameraSetting.sensitivity = sensitivitySlider.value + 0.3f;
                }

                if (FindAnyObjectByType<PlanningModeController>() == false)
                    gameHud.SetActive(true);
                else
                    planningGUI.SetActive(true);
               buttonPress = false;


            }
        }
    }
}
