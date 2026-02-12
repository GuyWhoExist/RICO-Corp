using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using System.Collections.Generic;

public class Shooting : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] private float maxDistance;
    [SerializeField] private LineRenderer lineRenderer; //displays the shot of the player - Nova
    [SerializeField] private LineRenderer lineRenderer2; //The same, but for the prediction instead - Nova
    [SerializeField] private Material planningMaterial;
    //private AudioSource effectPlayer;
    //[SerializeField] private AudioClip shot;
    private bool hitting = true;

    private LayerMask Collideable;
    private Vector3 shotOrigin;
    private Vector3 shotDirection;
    private Controls controls;
    [SerializeField] private int hits; //total number of bounces on the gun - Nova
    [SerializeField] private float travelTime; //time between rendering shots - Nova
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
    [SerializeField] private TextMeshProUGUI killstreakCounter;
    private Vector3 trackerPositionOrig;
    private float shakeInputRandom;
    private int CoinFlip;
    [SerializeField] private SpeedBoost speedBoost;
    private SightTracker trackerOfSight;
    
    

    [Header("Impact Decal Config")]
    [SerializeField] private bool impactDecals;
    [SerializeField] private bool reflectDecals;
    [SerializeField] private GameObject impactDecal;
    [SerializeField] private GameObject reflectDecal;
    private void Awake()
    {
        Collideable = LayerMask.GetMask("Default", "whatIsGround", "Ending");
        trackerOfSight = FindAnyObjectByType<SightTracker>();
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
        if (FindAnyObjectByType<PlanningModeController>() != null)
        {
            lineRenderer.material = planningMaterial;
            lineRenderer.material.renderQueue = 4000;
            impactDecals = false;
            reflectDecals = false;
        }
    }

    private void OnEnable()
    {
        controls.Guns.Shoot.Enable();
        controls.Guns.Shoot.performed += Shoot_performed;
        trackerPositionOrig = killstreakCounter.transform.position;
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

        shakeInputRandom = Random.Range((0.2f * killStreak) * -1, 0.2f * killStreak);
        CoinFlip = Random.Range(0, 2);
        
        if (killStreak == 0)
        {
            killstreakCounter.text = "       ";
            killstreakCounter.transform.position = trackerPositionOrig;
        }
        else
        {
            killstreakCounter.text = $"{killStreak}x";
            if (CoinFlip == 0)
            {
                killstreakCounter.transform.position = new Vector3(trackerPositionOrig.x + shakeInputRandom, trackerPositionOrig.y, trackerPositionOrig.z);
                //killStreakPositionLocker = true;
            }
            else if (CoinFlip == 1)
            {
                killstreakCounter.transform.position = new Vector3(trackerPositionOrig.x, trackerPositionOrig.y + shakeInputRandom, trackerPositionOrig.z);
            }
          
        }

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

        //if (boostCoolDownStored <= 0)
        //{
        //    if (killStreak > 0)
        //    {
        //        playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed - playerMovementTutorial.killBoost;
        //        killStreak = killStreak - 1;
        //        Debug.Log(killStreak);
        //        boostCoolDownStored = 1;
        //    }
        //}
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) //the ACTUAL SHOT - Nova
    {
        StopAllCoroutines();
        Vector3 trueOrigin = cam.transform.position;
        trueOrigin.y -= 0.5f;
        shotOrigin = cam.transform.position;
        //shotOrigin.y -= 0.5f;
        Vector3 temp3 = shotOrigin;
        List<Vector3> points = new List<Vector3>(); //stores all the impact points of the shot - Nova
        temp3.y -= 0.5f;
        shotDirection = cam.transform.forward;
        lineRenderer.positionCount = 1; //establishes the number of nodes of the line Renderer. - Nova
        hitting = true;
        bool dontDraw = false;
        int total = hits; //total is the number we actually modify when counting bounces
        //int color = 0;
        //effectPlayer.PlayOneShot(shot);
        while (hitting && total != 0)
        {
            Debug.Log("Started Timer");
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
                        points.Add(hit.point); //we give point we hit to a list we use later - Nova                     
                        shotOrigin = hit.point + shotDirection * 0.01f;//change the origin to where we hit + a tiny bit out to prevent the origin from being in a wall. - Nova
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
                        points.Add(hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        shotDirection = Vector3.Reflect(shotDirection, hit.normal);
                        total--;
                        //we simply reflect the shot, add to the line renderer and decrease total - Nova
                    }
                    if (total == 0) //if we hit a reflectable surface but are out of bounces... - Nova
                    {
                        Debug.Log("Hit bouce max");
                        if (impactDecals)
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
                        points.Add(hit.point);
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
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        Debug.Log("Enemy Hit");
                        if (trackerOfSight.seen == true)
                        {
                            trackerOfSight.seen = false;
                        }
                        if (total < hits)
                        {
                            Debug.Log(killStreak);
                            //playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed + playerMovementTutorial.killBoost;
                            speedBoost.fuel += 1f;
                            Debug.Log($"Fuel is at: {speedBoost.fuel}");
                            /*boostCoolDownStored = playerMovementTutorial.boostCoolDown;
                            Debug.Log($"{boostCoolDownStored}");
                            killStreak = killStreak + 1; */

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
                    if (impactDecals)
                        if (hit.transform.GetComponent<PlayerMovementTutorial>() == null && hit.transform.GetComponent<Rigidbody>() == null && hit.transform.GetComponent<BulletImpactPreventer>() == null)
                            Instantiate(impactDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    points.Add(hit.point);
                }
                   
            }
            else //if you dont hit anything - Nova
            {
                Debug.Log("Miss");
                //Debug.DrawRay(shotOrigin, shotDirection, colors[color], 1000);
                hitting = false;
                dontDraw = true;
            }
        }
        Debug.Log("Finished Shooting");
        Debug.Log($"LR Positions: {lineRenderer.positionCount}");
        Debug.Log($"List Positions: {points.Count}");
        if (!dontDraw)
        {
            StartCoroutine(PlacePoints(points, 2, trueOrigin, true, lineRenderer.positionCount));
        }
        
    }

    IEnumerator PlacePoints(List<Vector3> points, int times, Vector3 origin, bool first, int mos) //Animates the shot/places each ricochet individually after a dealy. - Nova
    {
        if (first) //sets the initial shot instantly - Nova
        {
            Debug.Log("First Shot Set");
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, points[0]);
        }
        yield return new WaitForSeconds(travelTime);
        if (times <= points.Count) //checks if we have gone through the entire list of impact points
        {
            Debug.Log("Change Position Count");
            lineRenderer.positionCount++; //we manually increase this to prevent the line renderer from drawing a line to (0,0,0) - Nova
            Debug.Log($"Using {times}");
            lineRenderer.SetPosition(times, points[times - 1]); //Sets the position of the latest point in the linder renderer - Nova
            times++; //used to track how many points we have used - Nova
            StartCoroutine(PlacePoints(points, times, origin, false, mos)); //We call PlacePoints again. This repeats until we use up all the points. - Nova
        }
        else //Begins the reset timer once we have used up all the points - Nova
        {
            if (FindAnyObjectByType<PlanningModeController>() == null)
            {
                Debug.Log("Begin Reset Timer");
                StartCoroutine(ResetShot());
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

