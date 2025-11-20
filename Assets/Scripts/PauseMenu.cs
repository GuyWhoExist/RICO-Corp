using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;

public class PauseMenu : MonoBehaviour
{
    private Controls controls;
    public bool paused;
    [SerializeField] GameObject pauseHud;
    [SerializeField] GameObject gameHud;
 

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
        if (paused == false)
        {
            Time.timeScale = 0;
            paused = true;
            gameHud.SetActive(false);
            pauseHud.SetActive(true);
            
            
        }
        else if (paused == true)
        {
            Time.timeScale = 1;
            paused = false;
            pauseHud.SetActive(false);
            gameHud.SetActive(true);
       
        }
    }
}
