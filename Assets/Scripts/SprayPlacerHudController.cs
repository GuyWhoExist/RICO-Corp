using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprayPlacerHudController : MonoBehaviour
{
    private Controls controls;
    [SerializeField] GameObject plannerUI;

    [Header("Shoot Marker")]
    [SerializeField] GameObject shootMarkerReflect;
    [SerializeField] GameObject shootMarker;
    [SerializeField] GameObject shootMarkerDestroy;
    [Header("Go Marker")]
    [SerializeField] GameObject goMarkerReflect;
    [SerializeField] GameObject goMarker;
    [SerializeField] GameObject goMarkerDestroy;
    [Header("Stop Marker")]
    [SerializeField] GameObject stopMarkerReflect;
    [SerializeField] GameObject stopMarker;
    [SerializeField] GameObject stopMarkerDestroy;

    [SerializeField] GameObject playerPosition;
    [HideInInspector] public bool selector;
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
        markerSelect = 0;
        //0 is default, 1 is attack, 2 is stop and 3 is follow.
    }

    private void Planner_Closed(InputAction.CallbackContext context)
    {
        plannerUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        selector = false;
    }

    private void Planner_Opened(InputAction.CallbackContext context)
    {
        plannerUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        selector = true;

    }

    private void OnDisable()
    {
        controls.Planning.MarkerUI.Disable();
    }

    public void OnShootPress()
    {
        markerSelect = 1;
        plannerUI.SetActive(false);

        if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, 10f))
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<TImeHazard>() == null && hit.transform.GetComponent<Sludge>() == null && hit.transform.GetComponent<Enemy>() == null)
            {

                
                    if (hit.transform.GetComponent<Reflect>() == true)
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(shootMarkerReflect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                    else if (hit.transform.GetComponent<Destroyable>() == true)
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(shootMarkerDestroy, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                    else
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(shootMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                
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
        plannerUI.SetActive(false);

        if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, 10f))
        {
            if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<TImeHazard>() == null && hit.transform.GetComponent<Sludge>() == null && hit.transform.GetComponent<Enemy>() == null)
            {
              
                    if (hit.transform.GetComponent<Reflect>() == true)
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(stopMarkerReflect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                    else if (hit.transform.GetComponent<Destroyable>() == true)
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(stopMarkerDestroy, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                    else 
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(stopMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                
           
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
        plannerUI.SetActive(false);

        if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, 10f))
        {
           if (hit.transform.GetComponent<Spray>() == null && hit.transform.GetComponent<Hazard>() == null && hit.transform.GetComponent<TImeHazard>() == null && hit.transform.GetComponent<Sludge>() == null && hit.transform.GetComponent<Enemy>() == null)
            {
                
                
                    if (hit.transform.GetComponent<Reflect>() == true)
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(goMarkerReflect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                    else if (hit.transform.GetComponent<Destroyable>() == true)
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(goMarkerDestroy, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
                    else 
                    {
                        //GameObject createdShootMarker = Instantiate(shootMarker, hit.point, Quaternion.identity);
                        Instantiate(goMarker, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        selector = false;
                        markerSelect = 0;
                    }
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
