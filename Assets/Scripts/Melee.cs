
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Melee : MonoBehaviour
{
    private Controls controls;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject playerPosition;
    RaycastHit hit;
    [SerializeField] float meleeRange;
    [SerializeField] Shooting shooting;
    private Vector3 swingDirection;
    [SerializeField] private float swingCoolDown;
    [SerializeField] private PlayerMovementTutorial playerMovementTutorial;
    private float swingCoolDownStored;


    private void Awake()
    {
        controls = new Controls();
        swingDirection = playerPosition.transform.forward;
        swingCoolDownStored = 0;
    }
    private void OnEnable()
    {
       controls.Melee.Swing .Enable();
        controls.Melee.Swing.performed += Swing_performed;
    }

    private void Swing_performed(InputAction.CallbackContext obj)
    {
        if (swingCoolDownStored < 0)
        {
            if (Physics.Raycast(playerPosition.transform.position, playerPosition.transform.forward, out hit, meleeRange))
            {
                if (hit.transform.TryGetComponent(out IShootable shootable))
                {
                    Destroy(shootable.GetGameObject());
                    Debug.Log("enemy SHOULD be bludgoned to death");
                    rb.AddForce((hit.point - playerPosition.transform.position).normalized * playerMovementTutorial.moveSpeed * 5f, ForceMode.Impulse);
                    shooting.killStreak = shooting.killStreak + 1;
                    playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed + playerMovementTutorial.killBoost;
                    shooting.boostCoolDownStored = playerMovementTutorial.boostCoolDown;
                }
                Debug.Log("swing raycast is fired");
            }
            Debug.Log("melee is swung");
            swingCoolDownStored = swingCoolDown;
        }
    }

    private void OnDisable()
    {
        controls.Melee.Swing.Disable();
    }

    private void Update()
    {
        swingCoolDownStored = swingCoolDownStored - Time.deltaTime;
    }
}
