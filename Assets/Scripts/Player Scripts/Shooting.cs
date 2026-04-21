using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class Shooting : MonoBehaviour
{
    //Controls how the gun fires and ricochets. The selling point.
    //Coded by Nova
    //also has code to assist functions of the enemy tracker arrow.
    //these additions were added by sawyer
    RaycastHit hit;
    [SerializeField] private float maxDistance;
    [SerializeField] private LineRenderer lineRenderer; //displays the shot of the player - Nova
    [SerializeField] private LineRenderer lineRenderer2; //The same, but for the prediction instead - Nova
    [SerializeField] private Material planningMaterial;
    //private AudioSource effectPlayer;
    //[SerializeField] private AudioClip shot;
    private bool hitting = true;
    public List <RifleEnemy> listOfActiveEnemies;
    public List<RifleEnemy> listOfTargetingEnemies;
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
    private float shotDelay;
    private SightTracker trackerOfSight;
    [SerializeField] private AudioSource SFXPlayer;
    [SerializeField] private AudioClip shotSFX;
    private RifleEnemy storedEnemy;
    
    

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
        if (FindAnyObjectByType<PlanningModeController>() != null)
        {
            lineRenderer.material = planningMaterial;
            lineRenderer.material.renderQueue = 4000;
            impactDecals = false;
            reflectDecals = false;
        }
      
        listOfActiveEnemies = new List<RifleEnemy>(FindObjectsByType<RifleEnemy>(FindObjectsSortMode.None));
        InvokeRepeating(nameof(EnemyStateTracker), 0.1f, 0.1f);
    }

    private void OnEnable()
    {
        controls.Guns.Shoot.Enable();
        controls.Guns.Shoot.performed += Shoot_performed;
        trackerPositionOrig = killstreakCounter.transform.position;
        shotDelay = -1;
    }
    private void OnDisable()
    {
        controls.Guns.Shoot.Disable();
        controls.Guns.Shoot.performed -= Shoot_performed;
    }

    private void EnemyStateTracker()//used to track states of tracked enemies. checked 10 times per second. this segment was made and commented by sawyer.
    {
        for (int i = 0; i < listOfActiveEnemies.Count; i++)//checks through the stored enemies that can attack
        {
            listOfActiveEnemies[i].listIndex = i;//sets the list indexes of the enemies to make storing them easier
            //Debug.Log(i);
            if (listOfActiveEnemies[i].activeState != 0)//checks if the enemy is tracking the player
            {
                listOfTargetingEnemies.Add(listOfActiveEnemies[i]);//if it is, adds it to the secondary list for attacking enemies
            }
            else if (listOfActiveEnemies[i] == null)//else if the enemy exists, and removes it if it doesnt (for killed enemies)
            {
                    listOfActiveEnemies.Remove(listOfActiveEnemies[i]);
            }
        }
        for (int i = 0; i < listOfTargetingEnemies.Count; i++)//chekcs through the targetting enemy list
        {
            listOfTargetingEnemies[i].targetListIndex = i;//sets the target list index just to help wit storage
            if (listOfTargetingEnemies[i].activeState == 0)//checks if the enemy in the list is still targeting
            {
                listOfTargetingEnemies.Remove(listOfTargetingEnemies[i]);//if its not removes it from the list.
            }

        }
    }
    private void Update() //everything in this is used for the PREDICTION LASER. Will probably go unused. - Nova        
        // there is now functions here aside from that - Sawyer
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
        shotDelay -= Time.deltaTime;
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
        if (shotDelay < 0)
        {

        
        Debug.Log("shoot");
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
                    SFXPlayer.PlayOneShot(shotSFX);
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
                            if (hit.transform.GetComponent<PlayerMovementTutorial>() == null && hit.transform.GetComponent<Rigidbody>() == null && hit.transform.GetComponent<BulletImpactPreventer>() == null) // verifies hit object is not player or rigidbody to avoid floating bulletholes - Sawyer
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
                            storedEnemy = hit.transform.GetComponent<RifleEnemy>();
                            EnemyKill();
                            shotOrigin = hit.point + shotDirection * 0.01f;
                        Debug.Log("Enemy Hit");
                        
                            

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
                if (total == hits)
                {
                    dontDraw = true;
                }
            }
        }
        Debug.Log("Finished Shooting");
        Debug.Log($"LR Positions: {lineRenderer.positionCount}");
        Debug.Log($"List Positions: {points.Count}");
        if (!dontDraw)
        {
            StartCoroutine(PlacePoints(points, 2, trueOrigin, true, lineRenderer.positionCount));
        }
            shotDelay = 0.7f;
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


    public void EnemyKill()//fires on the death of an enemy to avoid repeated code in several places.  this segment was made and commented by sawyer.
    {
        listOfActiveEnemies.Remove(storedEnemy);//removes the stored enemy from the list of all rifle enemies
        if (listOfTargetingEnemies.Contains(storedEnemy))
        {
            listOfTargetingEnemies.Remove(storedEnemy);//if it was targeting, remove it from that list as well
        }

        if (listOfTargetingEnemies.Count != 0)//makes certain there are still cached enemies
        {

            for (int i = 0; i < listOfTargetingEnemies.Count; i++)//if there are, runs through them. should select the first that appears.
            {
                if (listOfTargetingEnemies[i].activeState != 0 && listOfTargetingEnemies[i] != null)//verifies the enemy is actually tacking the player and not idle
                {
                    trackerOfSight.currentThreat = listOfTargetingEnemies[i].transform.position;//marks them as the new focus for the tracker
                }
                else
                {
                    trackerOfSight.UnSpotted();//disables the sight trackers visibility within its own scripts
                }

            }
            if (trackerOfSight.currentThreat != null)
            {
                trackerOfSight.UnSpotted();//disables the sight trackers visibility within its own scripts
            }

        }
        storedEnemy = null;//clears the stored enemy field

    }

}

