using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuickRestart : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private LevelEnder levelEnder;
    [SerializeField] private TimerController timerController;
    private LevelProgressTracker levelProgressTracker;
    public bool playerDie;
    private void Awake()
    {
        controls = new Controls();
        levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
        playerDie = false;
    }
    private void OnEnable()
    {
        controls.QuickRestart.Restart.Enable();
        controls.QuickRestart.Restart.performed += Restart_Performed;
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
    }
}
