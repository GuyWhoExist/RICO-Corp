using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SprayPlacerHudController : MonoBehaviour
{
    private Controls controls;
    [SerializeField] GameObject plannerUI;
    [SerializeField] GameObject playerPosition;
    [HideInInspector] public bool selector;
    [SerializeField] TextMeshProUGUI rotationCounter;
    [SerializeField] private GameObject rotationDisplay;
    private Quaternion rotationForm;
    private float rotationValue;

    [Header("Markers")]
    [SerializeField] GameObject shootMarker;
    [SerializeField] GameObject goMarker;
    [SerializeField] GameObject stopMarker;
    private GameObject placedMarker;



    private int markerSelect;
    private RaycastHit hit;
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
        controls.Planning.Rotate.performed += Rotation_Performed;
        controls.Planning.Rotate.canceled += Rotation_Ceased;
        markerSelect = 0;
        //0 is default, 1 is attack, 2 is stop and 3 is follow.
    }

    private void Rotation_Ceased(InputAction.CallbackContext context)
    {
      // uhhhhhh
    }

    private void Rotation_Performed(InputAction.CallbackContext context)
    {
        if (controls.Planning.Rotate.ReadValue<Vector2>().y > 0)
        {
            rotationValue += 15;
            if (rotationValue > 360)
                rotationValue = -345;
            rotationCounter.text = rotationValue.ToString();
            rotationDisplay.transform.rotation = new Quaternion(rotationDisplay.transform.rotation.x, rotationDisplay.transform.rotation.y, rotationDisplay.transform.rotation.z + 15, rotationDisplay.transform.rotation.w);
        }
        else
        {
            rotationValue -= 15;
            if (rotationValue < -360)
                rotationValue = 345;
            rotationCounter.text = rotationValue.ToString();
            rotationDisplay.transform.rotation = new Quaternion(rotationDisplay.transform.rotation.x, rotationDisplay.transform.rotation.y, rotationDisplay.transform.rotation.z - 15, rotationDisplay.transform.rotation.w);
        }
    }

    private void Planner_Closed(InputAction.CallbackContext context)
    {
        rotationCounter.text = " ";
        plannerUI.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        selector = false;
        controls.Planning.Rotate.Disable();
        rotationValue = 0;  
    }

    private void Planner_Opened(InputAction.CallbackContext context)
    {
        plannerUI.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        selector = true;
        controls.Planning.Rotate.Enable();

    }

    private void OnDisable()
    {
        controls.Planning.MarkerUI.Disable();
    }

    public void OnShootPress()
    {
        markerSelect = 1;
        rotationCounter.text = " ";
        plannerUI.SetActive(false);


        if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, 10f))
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<Enemy>() == null)
            {

                rotationForm = (Quaternion.FromToRotation(Vector3.forward, hit.normal));
                //Instantiate(shootMarker, hit.point, new Quaternion(rotationValue, rotationForm.y, rotationForm.z, rotationForm.w));
                Instantiate(shootMarker, hit.point, rotationForm);
                markerSelect = 0;
                rotationValue = 0;
                

            }
            else
            {
                Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }
    }
    public void OnStopPress()
    {
        markerSelect = 2;
        rotationCounter.text = " ";
        plannerUI.SetActive(false);

        if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, 10f))
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<Enemy>() == null)
            {

                rotationForm = (Quaternion.FromToRotation(Vector3.forward, hit.normal));
                //Instantiate(stopMarker, hit.point, new Quaternion(rotationValue, rotationForm.y, rotationForm.z, rotationForm.w));
                Instantiate(stopMarker, hit.point, rotationForm);
                selector = false;
                markerSelect = 0;
                rotationValue = 0;

            }
            else
            {
                Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }

        }
    }
    public void OnFollowPress()
    {
        markerSelect = 3;
        rotationCounter.text = " ";
        plannerUI.SetActive(false);

        if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, 10f))
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<Enemy>() == null)
            {
                placedMarker = Instantiate(goMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                if (rotationValue != 0)
                {

                    placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue);
                }
                    selector = false;
             markerSelect = 0;
             
            }
            else
            {
                Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }   
    }
    public void OnDeletePress()
    {
        if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, 10f))
        {
            if (hit.transform.TryGetComponent(out ICleanable Spray))
            {
                Destroy(Spray.GetGameObject());
            }
        }
    }
}
