using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprayPlacerHudController : MonoBehaviour
{
    private Controls controls;
    [SerializeField] GameObject plannerUI;
    [SerializeField] GameObject shootMarker;
    [SerializeField] GameObject goMarker;
    [SerializeField] GameObject stopMarker;
    [SerializeField] GameObject playerPosition;
    private bool placeMode;
    private int markerSelect;
    private void Awake()
    {
        
        controls = new Controls();
    }
    public void OnEnable()
    {
        plannerUI.SetActive(false);
        controls.Planning.MarkerUI.Enable();
        controls.Planning.MarkerUI.performed += Planner_Opened;
        controls.Planning.MarkerUI.canceled += Planner_Closed;
        placeMode = false;
        markerSelect = 0;
        //0 is default, 1 is attack, 2 is stop and 3 is follow.
    }

    private void Planner_Closed(InputAction.CallbackContext context)
    {
        plannerUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Planner_Opened(InputAction.CallbackContext context)
    {
      if (placeMode == false)
        {
            plannerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

      if (placeMode == true)
        {
            if (markerSelect == 1)
            {
                Instantiate(shootMarker).transform.localPosition = playerPosition.transform.position + playerPosition.transform.forward;
                placeMode = false;
                markerSelect = 0;
            }
            else if (markerSelect == 2)
            {
                Instantiate(stopMarker).transform.localPosition = playerPosition.transform.position + playerPosition.transform.forward;
                placeMode = false;
                markerSelect = 0;
            }
            else if (markerSelect == 3)
            {
                Instantiate(goMarker).transform.localPosition = playerPosition.transform.position + playerPosition.transform.forward;
                placeMode = false;
                markerSelect = 0;
            }
            else
            {
                Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }

    }

    private void OnDisable()
    {
        controls.Planning.MarkerUI.Disable();
    }

    public void OnShootPress()
    {
       
        placeMode = true;
        markerSelect = 1;
        plannerUI.SetActive(false);
    }

    public void OnStopPress()
    {
        
        placeMode = true;
        markerSelect = 2;
        plannerUI.SetActive(false);
    }

    public void OnFollowPress()
    {
       
        placeMode = true;
        markerSelect = 3;
        plannerUI.SetActive(false);
    }

    private void Update()
    {
        if (placeMode == true)
        {
            
        }
    }
}
