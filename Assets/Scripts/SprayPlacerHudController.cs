using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SprayPlacerHudController : MonoBehaviour
{
    //coded by sawyer
    private Controls controls;
    [SerializeField] GameObject plannerUI;
    [SerializeField] GameObject CameraPosition;
    [SerializeField] Shooting shootingDisabler;
    [HideInInspector] public bool selector;
    [SerializeField] TextMeshProUGUI rotationCounter;
    [SerializeField] private UnityEngine.UI.RawImage rotationDisplay;
    private Quaternion rotationForm;
    private float rotationValue;
    private float invertedRotationValue;
    private float cameraAngle;
    [HideInInspector] public bool planningModeToggle;
    LayerMask sprayDetection;

    [Header("Markers")]
    [SerializeField] GameObject shootMarker;
    [SerializeField] GameObject goMarker;
    [SerializeField] GameObject stopMarker;
    private GameObject placedMarker;
    [HideInInspector] public GameObject collectedHit;



    private int markerSelect;
    [HideInInspector]public RaycastHit hit;
    private void Awake()
    {
        controls = new Controls();
        sprayDetection = LayerMask.GetMask("spray");
        collectedHit = gameObject;
    }
    public void OnEnable()
    {
        plannerUI.SetActive(false);
        if (FindAnyObjectByType<PlanningModeController>())
        {
            controls.Planning.MarkerUI.Enable();
            controls.Planning.MarkerUI.performed += Planner_Opened;
            controls.Planning.MarkerUI.canceled += Planner_Closed;
            controls.Planning.Rotate.performed += Rotation_Performed;
            controls.Planning.Rotate.canceled += Rotation_Ceased;
            markerSelect = 0;
        }
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
            invertedRotationValue -= 15;
            if (rotationValue > 360)
                rotationValue = -345;
            rotationCounter.text = rotationValue.ToString();
            rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, invertedRotationValue);
        }
        else
        {
            rotationValue -= 15;
            invertedRotationValue += 15;
            if (rotationValue < -360)
                rotationValue = 345;
            rotationCounter.text = rotationValue.ToString();
            rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, invertedRotationValue);
        }
    }
    private void Planner_Closed(InputAction.CallbackContext context)
    {
        rotationCounter.text = " ";
        invertedRotationValue = 0;
        rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, 0);
        plannerUI.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        selector = false;
        controls.Planning.Rotate.Disable();
        rotationValue = 0;
        shootingDisabler.spraying = false;
        
    }
    private void Planner_Opened(InputAction.CallbackContext context)
    {
        cameraAngle = CameraPosition.transform.rotation.eulerAngles.y + 180;
            plannerUI.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        selector = true;
        controls.Planning.Rotate.Enable();
        shootingDisabler.spraying = true;
    }
    private void OnDisable()
    {
        controls.Planning.MarkerUI.Disable();
    }
    public void OnShootPress()
    {
        markerSelect = 1;
        rotationCounter.text = " ";
        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))
            collectedHit = hit.transform.gameObject;
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<Enemy>() == null)
            {
                placedMarker = Instantiate(shootMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                {
                    if (placedMarker.transform.rotation.eulerAngles.x != 0)
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue + cameraAngle);
                    }
                    else
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue);
                    }
                }
                selector = false;
                markerSelect = 0;
            }
            else
            {
                Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }
        plannerUI.SetActive(false);
    }
    public void OnStopPress()
    {
        markerSelect = 2;
        rotationCounter.text = " ";

        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))
            collectedHit = hit.transform.gameObject;
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<Enemy>() == null)
            {
                placedMarker = Instantiate(stopMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                {
                    if (placedMarker.transform.rotation.eulerAngles.x != 0)
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue + cameraAngle);
                    }
                    else
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue);
                    }
                }
                selector = false;
                markerSelect = 0;
            }
            else
            {
                Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }
        plannerUI.SetActive(false);
    }
    public void OnFollowPress()
    {
        markerSelect = 3;
        rotationCounter.text = " ";
       
        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))
            collectedHit = hit.transform.gameObject;
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<Enemy>() == null)
            {
                placedMarker = Instantiate(goMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                {
                    if (placedMarker.transform.rotation.eulerAngles.x != 0)
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue + cameraAngle);
                    }
                    else
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue);
                    }
                }
                selector = false;
                markerSelect = 0;
            }
            else
            {
                Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }
        plannerUI.SetActive(false);
    }
    public void OnDeletePress()
    {
        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))
        {
            if (hit.transform.TryGetComponent(out ICleanable Spray))
                Destroy(Spray.GetGameObject());
        }
    }
}
