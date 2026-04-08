using UnityEngine;
using UnityEngine.InputSystem;

public class SprayPlacerHudController : MonoBehaviour
{
    //coded by sawyer
    private Controls controls;//used to store themain controls
    [SerializeField] GameObject plannerUI;//used to store the UI foor the planner
    [SerializeField] GameObject CameraPosition;//used to store the cameras facing direction for raycasting
    [SerializeField] Shooting shootingDisabler;//used to disable shooting when firing a spray
    [HideInInspector] public bool selector;//used for detecting the UI status
    [SerializeField] private UnityEngine.UI.RawImage rotationDisplay;//used to store the rotation display
    private float rotationValue;//the internal value for rotating the sprays by a set amount
    private float invertedRotationValue;//the internal value for rotating the sprays in the opposite direction
    private float cameraAngle;//the angle of the players camera
    [HideInInspector] public bool planningModeToggle;//used for when planning mode is enabled
    LayerMask sprayDetection;//used to avoid stacking sprays
    [Header("Markers")]//the below are simply the actual decals the player can place
    [SerializeField] GameObject shootMarker;//the target marker
    [SerializeField] GameObject goMarker;//the arrow markere
    [SerializeField] GameObject stopMarker;//the stop sign marker
    private GameObject placedMarker;//the marker the player hasjust placed
    [HideInInspector] public GameObject collectedHit;//the stored gameobject the spray placement raycast has hit



    private int markerSelect;//the selected marker (used for save systems)
    [HideInInspector]public RaycastHit hit;//the position the raycast collided at
    private void Awake()//used to get controls and layermasks
    {
        controls = new Controls();//the controls
        sprayDetection = LayerMask.GetMask("spray");//the spray layermask
    }
    public void OnEnable()
    {
        plannerUI.SetActive(false);//disables the planner UI object
        if (FindAnyObjectByType<PlanningModeController>())//checks if the planning mode controller exists (if it does planning mode is enabled)
        {
            controls.Melee.Swing.Enable();//if planning mode is on, hijack the melee input for placing decals
            controls.Melee.Swing.performed += Planner_Opened;//sets the initial melee swing to open the planner
            controls.Melee.Swing.canceled += Planner_Closed;//sets the utton release to closing the planner
            controls.Planning.Rotate.performed += Rotation_Performed;//used to detect the player scrolling to rotate the spray placement
            controls.Planning.Rotate.canceled += Rotation_Ceased;//exists to appease the unity codebase, disregard
            markerSelect = 0;//used to save sprays.value is initially cleared to avoid false positives
        }
        //0 is default, 1 is attack, 2 is stop and 3 is follow.
    }

    private void Rotation_Ceased(InputAction.CallbackContext context)
    {
      // this exists to appease the unity codebase. disregard at is the player not scrolling anymore is irrelevant.
    }

    private void Rotation_Performed(InputAction.CallbackContext context)
    {
        if (controls.Planning.Rotate.ReadValue<Vector2>().y > 0)
        {
            rotationValue += 15;
            invertedRotationValue -= 15;
            if (rotationValue > 360)
                rotationValue = -345;
            rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, invertedRotationValue);
        }
        else
        {
            rotationValue -= 15;
            invertedRotationValue += 15;
            if (rotationValue < -360)
                rotationValue = 345;
            rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, invertedRotationValue);
        }
    }
    private void Planner_Closed(InputAction.CallbackContext context)
    {
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
        controls.Melee.Swing.Disable();
        controls.Melee.Swing.performed += Planner_Opened;
        controls.Melee.Swing.canceled += Planner_Closed;
        controls.Planning.Rotate.performed += Rotation_Performed;
        controls.Planning.Rotate.canceled += Rotation_Ceased;
    }
    public void OnShootPress()
    {
        markerSelect = 1;
        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))
        {
            collectedHit = hit.transform.gameObject;
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
                //Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }
        plannerUI.SetActive(false);
    }
    public void OnStopPress()
    {
        markerSelect = 2;

        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))
        {
            collectedHit = hit.transform.gameObject;
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
                //Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
            }
        }
        plannerUI.SetActive(false);
    }
    public void OnFollowPress()
    {
        markerSelect = 3;
       
        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))
        {
            collectedHit = hit.transform.gameObject;
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
                //Debug.Log($"Something has gone horrifyingly wrong in the markers, value: {markerSelect} ");
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
