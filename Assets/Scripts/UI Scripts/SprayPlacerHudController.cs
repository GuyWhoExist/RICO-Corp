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



    private int markerSelect;//the selected marker (used to tell the code what marker the player placed
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

            markerSelect = 0;//used to save sprays.value is initially cleared to avoid false positives
        }
        //0 is default, 1 is attack, 2 is stop and 3 is follow.
    }

    private void Rotation_Performed(InputAction.CallbackContext context)
    {
        if (controls.Planning.Rotate.ReadValue<Vector2>().y > 0)//used to check the direction the player is scrolling
        {
            rotationValue += 15;//if the player is scrolling up, rotate the spray clockwise
            invertedRotationValue -= 15;//set the inverse rotation value so that the rotation functions
            rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, invertedRotationValue);//rotates the visual display
        }
        else//if the player scrolls down
        {
            rotationValue -= 15;//rotate the spray counter-clockwise
            invertedRotationValue += 15;//set the inverse rotation value so that the rotation functions
            rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, invertedRotationValue);//rotate the visual display
        }
    }
    private void Planner_Closed(InputAction.CallbackContext context)
    {
        invertedRotationValue = 0;//clears the inverted rotation value 
        rotationDisplay.transform.rotation = Quaternion.Euler(rotationDisplay.transform.rotation.eulerAngles.x, rotationDisplay.transform.rotation.eulerAngles.y, 0);//resets the rotation of the visual display
        plannerUI.SetActive(false);//disables the planner UI
        Cursor.lockState = CursorLockMode.Locked;//locks the cursor
        selector = false;//disables the selector bool
        controls.Planning.Rotate.Disable();//disables the rotation input
        rotationValue = 0;//resets the rotation value
        shootingDisabler.spraying = false;//marks the player as no longer using the spray, thus enabling the players ability to shoot again.
    }
    private void Planner_Opened(InputAction.CallbackContext context)
    {
        cameraAngle = CameraPosition.transform.rotation.eulerAngles.y + 180;//gets the cameras rotation
        plannerUI.SetActive(true);//enables the planner UI
       Cursor.lockState = CursorLockMode.None;//unlocks the cursor
        selector = true;//marks the selector as enabled
        controls.Planning.Rotate.Enable();//enables the rotation functions
        shootingDisabler.spraying = true;//disables the shootting function
    }
    private void OnDisable()
    {
        controls.Melee.Swing.Disable();//disables the melee input
        controls.Melee.Swing.performed += Planner_Opened;//disables the planner opening feature
        controls.Melee.Swing.canceled += Planner_Closed;//disables the closing planner feature
        controls.Planning.Rotate.performed += Rotation_Performed;//disables the scrollwheel input

    }
    public void OnShootPress()
    {
        markerSelect = 1;//tells the system the player is placing marker 1
        PlaceSpray();//fires the function to place the decal
    }
    public void OnStopPress()
    {
        markerSelect = 2;//tells the system the player is placing marker 2
        PlaceSpray();//fires the function to place the decal
    }
    public void OnFollowPress()
    {
        markerSelect = 3;//tells the system the player is placing marker 3
        PlaceSpray();//fires the function to place the decal
    }

    private void PlaceSpray()
    {
        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))//fires a raycast to locate the position the player is firing the spray at.
        {
            collectedHit = hit.transform.gameObject;//stores the object the raycast hit
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<Enemy>() == null)//verifies the player is not attempting to spray on an instakill plane, enemy, or another spray.
            {

                if (markerSelect == 1)//checks if the player is trying to place a target marker
                {
                    placedMarker = Instantiate(shootMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));//places the target marker where the raycast hit and rotates it to fit to the face of the object
                }
                else if (markerSelect == 2)//otherwise, checks if the player is trying to place a stop marker
                {
                    placedMarker = Instantiate(stopMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));//places the stop marker where the raycast hit and rotates it to fit to the face of the object
                }
                else// if neither of those are true, it is guarunteed to be an arrow marker selected.
                {
                    placedMarker = Instantiate(goMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));//places the arrow marker where the raycast hit and rotates it to fit to the face of the object
                }

                {
                    if (placedMarker.transform.rotation.eulerAngles.x != 0)//checks if the spray has been placed on the ground
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue + cameraAngle);//rotates the placed marker to orient to the camera
                    }
                    else//if the spray is not on the ground
                    {
                        placedMarker.transform.localRotation = Quaternion.Euler(placedMarker.transform.rotation.eulerAngles.x, placedMarker.transform.rotation.eulerAngles.y, rotationValue);//place the spray normally
                    }
                }
                selector = false;//maarks the selector state as false
                markerSelect = 0;//clears the marker selection
            }
        }
        plannerUI.SetActive(false);//disables the planner hud
    }
    public void OnDeletePress()  
    {
        if (Physics.Raycast(CameraPosition.transform.position, CameraPosition.transform.forward, out hit, 10f))//fires out a raycast
        {
            if (hit.transform.TryGetComponent(out ICleanable Spray))//checks if the shot object is a spray
                Destroy(Spray.GetGameObject());//if it is a spray, deletes the object from the scene
        }
    }
}
