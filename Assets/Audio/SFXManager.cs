using UnityEngine;

public class SFXManager : MonoBehaviour
{
    //doesnt work :shrug: - Nova

    private Controls controls;


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        controls.Guns.Shoot.Enable();
        controls.Guns.Shoot.performed += Shoot_performed;
    }
    private void OnDisable()
    {
        controls.Guns.Shoot.Disable();
        controls.Guns.Shoot.performed -= Shoot_performed;
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }
}
