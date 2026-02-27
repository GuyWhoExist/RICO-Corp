using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//functionality for the quick restart. (Key that lets the player instantly restart without needing to open the pause menu) - Nova
//Sawyer made this one.

public class QuickRestart : MonoBehaviour
{
    private Controls controls;
    private LevelEnder levelEnder;
    [SerializeField] PlayerCamera killCancel;
    [SerializeField] TimerController timerController;
    private LevelProgressTracker levelProgressTracker;
    [HideInInspector] public bool playerDie;

    private bool anotherOverflowBlock; // stops the kill input from being generated every frame - Sawyer

    private void Awake()
    {
        controls = new Controls();
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        levelEnder = FindAnyObjectByType<LevelEnder>();
        playerDie = false;
    }
    private void OnEnable()
    {
        Debug.Log("kill exists");
        controls.QuickRestart.Restart.Enable();
        controls.QuickRestart.Restart.performed += Restart_Performed;
        anotherOverflowBlock = true;
    }
    private void OnDisable()
    {
        controls.QuickRestart.Restart.Disable();
    }
    private void Restart_Performed(InputAction.CallbackContext context)
    {
       playerDie = true;
    }

    private void Update()
    {
        
         if (playerDie == true)
         {
            if (levelEnder !=  null)
            {
                if (levelEnder.nextLevelIndex == 0)
                {
                    Debug.Log("Git gud");
                    timerController.end = false;
                    SceneManager.LoadScene(levelProgressTracker.levels.Length + 1);
                }
                else
                {
                    Debug.Log("Git gud");
                    timerController.end = false;
                    SceneManager.LoadScene(levelEnder.GetNextIndex() - 1);
                    Debug.Log("this is fine actually");
                }
            }
         else
            {
                Debug.Log("Currently In Invalid Scene. Sending to Menu");
                SceneManager.LoadScene(0);
            }

         playerDie = false;
         }

        if (killCancel.overflowBlock == false)
        {
            controls.QuickRestart.Restart.Disable();
            anotherOverflowBlock = false;
        }
        else if (killCancel.overflowBlock && anotherOverflowBlock == false)
        {
            controls.QuickRestart.Restart.Enable();
            anotherOverflowBlock = true;
        }

    }
    

}
