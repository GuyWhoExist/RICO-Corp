using System;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;

public class PauseMenu : MonoBehaviour
{
    private Controls controls;
    public bool paused;
    [SerializeField] PauseMenuController pauseController;
    [SerializeField] GameObject pauseHud;
    [SerializeField] GameObject gameHud;
    public bool buttonPress;
    //allows to unpause via other means
 

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        pauseHud.SetActive(false);
        controls.Pause.Pause.Enable();
        paused = false;
        controls.Pause.Pause.performed += Pause_Performed;
        controls.Pause.Pause.performed += (ctx) => Debug.Log("man");
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
        if (buttonPress == true)
        {
            if (paused == false)
            {
                Time.timeScale = 0;
                paused = true;
                gameHud.SetActive(false);
                pauseHud.SetActive(true);
                buttonPress = false;
            }
            else if (paused == true)
            {
                Time.timeScale = 1;
                paused = false;
                pauseHud.SetActive(false);
                gameHud.SetActive(true);
                buttonPress = false;

            }
        }
    }
}
