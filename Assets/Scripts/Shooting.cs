using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Shooting : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] private float maxDistance;
    [SerializeField] private LineRenderer lineRenderer; //displays the shot of the player - Nova
    [SerializeField] private LineRenderer lineRenderer2; //The same, but for the prediction instead - Nova
    //private AudioSource effectPlayer;
    //[SerializeField] private AudioClip shot;
    private bool hitting = true;

    private LayerMask Collideable;
    private Vector3 shotOrigin;
    private Vector3 shotDirection;
    private Controls controls;
    [SerializeField] private int hits; //total number of bounces on the gun - Nova
    private Color[] colors = new Color[6];
    [SerializeField] private Camera cam;
    [HideInInspector] public Enemy[] enemyNumber; //the total number of enemies
    [SerializeField] private bool prediction;
    public int killStreak;
    [SerializeField] private PlayerMovementTutorial playerMovementTutorial;
    public float boostCoolDownStored;
    [SerializeField] private PauseMenu pauseMenu;
    public bool spraying;
    [SerializeField] private TimerController timerController;
    private bool overflowBlock;
    
    

    [Header("Impact Decal Config")]
    [SerializeField] private bool impactDecals;
    [SerializeField] private bool reflectDecals;
    [SerializeField] private GameObject impactDecal;
    [SerializeField] private GameObject reflectDecal;
    private void Awake()
    {
        Collideable = LayerMask.GetMask("Default", "whatIsGround", "Ending");
        controls = new Controls();
        lineRenderer = GetComponent<LineRenderer>();
        //effectPlayer = GetComponent<AudioSource>();
        shotOrigin = transform.position;
        shotDirection = transform.forward;
        lineRenderer.SetPosition(0, transform.position);
        colors[0] = Color.red;
        colors[1] = Color.yellow;
        colors[2] = Color.green;
        colors[3] = Color.cyan;
        colors[4] = Color.blue;
        colors[5] = Color.magenta;
        enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
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

    private void Update() //everything in this is used for the PREDICTION LASER. - Nova
    {
        // allows to disable shooting when using any user interface, uis must be manually added
        if (pauseMenu.paused == true || timerController.end == true || spraying == true)
        {
            controls.Guns.Shoot.Disable();
            overflowBlock = false;
        }
        else if (pauseMenu.paused == false && overflowBlock == false || timerController.end && overflowBlock == false || spraying == false && overflowBlock == false)
        {
            controls.Guns.Shoot.Enable();
            overflowBlock = true;
        }
        //end


        shotOrigin = cam.transform.position;
        shotOrigin.y -= 0.5f;
        shotDirection = cam.transform.forward;
        lineRenderer2.positionCount = 1;
        lineRenderer2.SetPosition(0, shotOrigin);
        lineRenderer2.material = lineRenderer2.materials[0];
        hitting = true;
        int total = hits;
        //int color = 0;
        while (hitting && total != 0 && prediction)
        {
            //lineRenderer.alignment = LineAlignment.TransformZ;
            if (Physics.Raycast(shotOrigin, shotDirection, out hit, maxDistance))
            {
                if (hit.transform.GetComponent<Reflect>() != null)
                {
                    if (hit.transform.TryGetComponent(out IShootable shoot))
                    {
                        lineRenderer2.positionCount++;
                        lineRenderer2.SetPosition(lineRenderer2.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        shotDirection = Vector3.Reflect(shotDirection, hit.normal);
                        total--;
                    }
                    else
                    {
                        lineRenderer2.positionCount++;
                        lineRenderer2.SetPosition(lineRenderer2.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        shotDirection = Vector3.Reflect(shotDirection, hit.normal);
                        total--;
                    }
                    if (total == 0)
                    {
                    }
                }
                else if (hit.transform.TryGetComponent(out IShootable shootable))
                {
                    if (hit.transform.GetComponent<Destroyable>() != null)
                    {
                        lineRenderer2.positionCount++;
                        lineRenderer2.SetPosition(lineRenderer2.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                    }
                    else
                    {
                        lineRenderer2.positionCount++;
                        lineRenderer2.SetPosition(lineRenderer2.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        lineRenderer2.material = lineRenderer2.materials[1];
                        Debug.Log(lineRenderer2.materials);
                    }
                }

                else
                {
                    //Debug.DrawRay(shotOrigin, shotDirection, colors[color], 1000);
                    hitting = false;
                    lineRenderer2.positionCount++;
                    lineRenderer2.SetPosition(lineRenderer2.positionCount - 1, hit.point);
                }
            }
            else
            {
                //Debug.DrawRay(shotOrigin, shotDirection, colors[color], 1000);
                hitting = false;
            }
            Debug.Log(lineRenderer2.material.ToString());
        }

        boostCoolDownStored = boostCoolDownStored - Time.deltaTime;

        if (boostCoolDownStored <= 0)
        {
            if (killStreak > 0)
            {
                playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed - playerMovementTutorial.killBoost;
                killStreak = killStreak - 1;
                Debug.Log(killStreak);
            }
        }
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) //the ACTUAL SHOT - Nova
    {
        StopAllCoroutines(); 
        shotOrigin = cam.transform.position;
        //shotOrigin.y -= 0.5f;
        Vector3 temp3 = shotOrigin;
        temp3.y -= 0.5f;
        shotDirection = cam.transform.forward;
        lineRenderer.positionCount = 1; //establishes the number of nodes of the line Renderer. - Nova
        lineRenderer.SetPosition(0, temp3);
        hitting = true;
        int total = hits; //total is the number we actually modify when counting bounces
        //int color = 0;
        //effectPlayer.PlayOneShot(shot);
        while (hitting && total != 0)
        {

         
            #region test :D
            //this is a region, use this
            #endregion
            //lineRenderer.alignment = LineAlignment.TransformZ;
            if (Physics.Raycast(shotOrigin, shotDirection, out hit, maxDistance, Collideable)) //initial raycast check - Nova
            {
                if (hit.transform.GetComponent<Reflect>() != null) //if we hit a reflective surface... (these are the only outcomes that decrease the remaining number of bounces)- Nova
                {
                    if (hit.transform.TryGetComponent(out IShootable shoot)) //we check if it is a shootable target - Nova
                    {
                        //im just gonna explain how the reflect code works here and point out any changes in other blocks - Nova
                        lineRenderer.positionCount++; //we increase the number of nodes in the line renderer - Nova
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point); //we give the node a position, being where we hit - Nova
                        shotOrigin = hit.point + shotDirection * 0.01f; //change the origin to where we hit + a tiny bit out to prevent the origin from being in a wall. - Nova
                        shotDirection = Vector3.Reflect(shotDirection, hit.normal); //and change its direction based on the angle of the surface - Nova
                        Debug.Log("Armored Glass Hit"); //the only reflectable and shootable thing is armored glass - Nova
                        if (FindAnyObjectByType<PlanningModeController>() == null)
                        {
                            shoot.OnGettingShot(); //then we run the shootable target's OnGettingShot function - Nova
                        }
                        total--; //then decrease the total # of bounces left - Nova
                    }
                    else //if the surface is only reflectable... - Nova
                    {
                        lineRenderer.positionCount++;
                        if (reflectDecals) // allows toggling reflect toggling
                            if (hit.transform.GetComponent<PlayerMovementTutorial>() == null && hit.transform.GetComponent<Rigidbody>() == null && hit.transform.GetComponent<BulletImpactPreventer>() == null ) // verifies hit object is not player or rigidbody to avoid floating bulletholes - Sawyer
                                Instantiate(reflectDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));// places the reflect based bullet hole - Sawyer
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        shotDirection = Vector3.Reflect(shotDirection, hit.normal);
                        total--;
                        //we simply reflect the shot, add to the line renderer and decrease total - Nova
                    }
                    if (total == 0) //if we hit a reflectable surface but are out of bounces... - Nova
                    {
                        Debug.Log("Hit bouce max");
                        if (impactDecal)
                            if (hit.transform.GetComponent<PlayerMovementTutorial>() == null && hit.transform.GetComponent<Rigidbody>() == null && hit.transform.GetComponent<BulletImpactPreventer>() == null)
                                Instantiate(impactDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        //we simply end it. - Nova
                    }
                }
                else if (hit.transform.TryGetComponent(out IShootable shootable)) //if the object isnt reflectable but is still a shootable... (the gun PIERCES non reflectable targets) - Nova
                {
                    if (hit.transform.GetComponent<Destroyable>() != null) //glass has the destroyable component - Nova
                    {
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        Debug.Log("Glass Hit");
                        if (FindAnyObjectByType<PlanningModeController>() == null)
                        {
                            shootable.OnGettingShot();
                        }
                        //we add a node to the line renderer, but we DONT decrease the total, as this isnt a bounce - Nova
                        //we also dont reflect the shot and keep the direction the same to give the effect of piercing - Nova
                    }
                    else //enemies ONLY have shootable - Nova
                    {
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        Debug.Log("Enemy Hit");
                        if (total < hits)
                        {
                            Debug.Log(killStreak);
                            playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed + playerMovementTutorial.killBoost;
                            boostCoolDownStored = playerMovementTutorial.boostCoolDown;
                            Debug.Log($"{boostCoolDownStored}");
                            killStreak = killStreak + 1;
                        }
                        if (FindAnyObjectByType<PlanningModeController>() == null)
                        {
                            shootable.OnGettingShot();
                            Destroy(shootable.GetGameObject()); // this kills the enemy? ig? - Sawyer
                        }
                        enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None); //reduces the enemy count - Nova
                    }
                }

                else //if we hit something that isnt a destroyable or reflectable surface we end the shot;
                {
                    Debug.Log("Non reflect/enemy hit");
                    //Debug.DrawRay(shotOrigin, shotDirection, colors[color], 1000);
                    hitting = false;
                    lineRenderer.positionCount++;
                    if (impactDecal)
                        if (hit.transform.GetComponent<PlayerMovementTutorial>() == null && hit.transform.GetComponent<Rigidbody>() == null && hit.transform.GetComponent<BulletImpactPreventer>() == null)
                            Instantiate(impactDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                }
                if (FindAnyObjectByType<PlanningModeController>() == null)
                {
                    StartCoroutine(ResetShot());
                }   
            }
            else //if you dont hit anything - Nova
            {
                Debug.Log("Miss");
                //Debug.DrawRay(shotOrigin, shotDirection, colors[color], 1000);
                hitting = false;
            }
        }
    }

    IEnumerator ResetShot() //resets the line renderer after 2 seconds
    {
        yield return new WaitForSeconds(2f);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, shotOrigin);
    }
}

