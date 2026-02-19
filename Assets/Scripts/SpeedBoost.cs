using UnityEngine;

public class SpeedBoost : MonoBehaviour
{

    //This script allows the player to activate a speed boost - Nova
    //Coded by Nova

    private Controls controls;
    
    [HideInInspector] public float fuel; //stores how much boost we have - Nova
    [SerializeField] private float fuelLimit; //how much boost we can hold at once - Nova
    private float startSpeed; //tracks the player's starting movement speed - Nova
    private float boostRemaining; //used to track how much time we have left on the boost - Nova
    [SerializeField] private float boost; //how big the boost is - Nova
    [SerializeField] private float boostDuration; //how long the boost lasts - Nova
    [SerializeField] private PlayerMovementTutorial playerMovement;
    [SerializeField] private Shooting shooting;
    private bool planningMode;


    private void OnEnable()
    {
        controls.Movement.Boost.Enable();
        controls.Movement.Boost.performed += Boost_performed;
        if (FindAnyObjectByType<PlanningModeController>())
        {
            planningMode = true;
        }
        else
        {
            planningMode = false;
        }
    }
    private void OnDisable()
    {
        controls.Movement.Boost.Disable();
        controls.Movement.Boost.performed -= Boost_performed;
    }

    private void Boost_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("We have read the dash input");
        if (fuel >= 1)
        {
            Debug.Log("Boost Succesful");
            boostRemaining += boostDuration;
            fuel--;
        }
        else if(planningMode == true)
        {
            boostRemaining += boostDuration;
        }
        else
        {
            Debug.Log("Not Enough Fuel");
        }
    }

    private void Update()
    {
        if (boostRemaining > boostDuration*2 && playerMovement.grounded) //checks for each level of boost, capping out at x3 boost - Nova
        {
            Debug.Log("Boost at x3");
            playerMovement.moveSpeed = startSpeed + (boost * 3);
            shooting.killStreak = 3;
        }
        else if (boostRemaining > boostDuration && playerMovement.grounded)
        {
            Debug.Log("Boost at x2");
            playerMovement.moveSpeed = startSpeed + (boost * 2);
            shooting.killStreak = 2;
        }
        else if (boostRemaining > 0 && playerMovement.grounded)
        {
            Debug.Log("Boost at x1");
            playerMovement.moveSpeed = startSpeed + boost;
            shooting.killStreak = 1;
        }

        if (boostRemaining > 0)
        {
            boostRemaining -= Time.deltaTime;
        }
        else //just to make sure we are starting at 0 - Nova
        {
            shooting.killStreak = 0;
            boostRemaining = 0f;
            playerMovement.moveSpeed = startSpeed;
        }

        if (fuel > fuelLimit) //adds a cap to how many boost we can hold - Nova
        {
            Debug.Log("Discarding excess fuel");
            fuel = fuelLimit;
        }
    }

    private void Awake()
    {
        startSpeed = playerMovement.moveSpeed;
        controls = new Controls();
    }
}
