using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] private float maxDistance;
    [SerializeField] private LineRenderer lineRenderer; //displays the shot of the player - Nova
    [SerializeField] private LineRenderer lineRenderer2; //The same, but for the prediction instead - Nova
    //private AudioSource effectPlayer;
    //[SerializeField] private AudioClip shot;
    private bool hitting = true;
    private Vector3 shotOrigin;
    private Vector3 shotDirection;
    private Controls controls;
    [SerializeField] private int hits;
    private Color[] colors = new Color[6];
    [SerializeField] private Camera cam;
    [HideInInspector] public Enemy[] enemyNumber; //the total number of enemies
    [SerializeField] private bool prediction;
    private int killStreak;
    [SerializeField] private PlayerMovementTutorial playerMovementTutorial;
    public float boostCoolDownStored;
    [SerializeField] private PauseMenu pauseMenu;
    private bool overflowBlock;

    private void Awake()
    {

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
        // allows to disable shoot
        if (pauseMenu.paused == true)
        {
            controls.Guns.Shoot.Disable();
            overflowBlock = false;
        }
        else if (pauseMenu.paused == false && overflowBlock == false)
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

        boostCoolDownStored -= Time.deltaTime;

        if (boostCoolDownStored <= 0)
        {
            if (killStreak > 0)
            {
                playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed - 1;
                killStreak = killStreak - 1;
                Debug.Log(killStreak);
            }
        }
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) //the ACTUAL SHOT - Nova
    {
        StopAllCoroutines(); 
        shotOrigin = cam.transform.position;
        shotOrigin.y -= 0.5f;
        shotDirection = cam.transform.forward;
        lineRenderer.positionCount = 1; //establishes the number of nodes of the line Renderer. - Nova
        lineRenderer.SetPosition(0, shotOrigin);
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
            if (Physics.Raycast(shotOrigin, shotDirection, out hit, maxDistance)) //initial raycast check - Nova
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
                        shoot.OnGettingShot(); //then we run the shootable target's OnGettingShot function - Nova
                        total--; //then decrease the total # of bounces left - Nova
                    }
                    else //if the surface is only reflectable... - Nova
                    {
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        shotDirection = Vector3.Reflect(shotDirection, hit.normal);
                        total--;
                        //we simply reflect the shot, add to the line renderer and decrease total - Nova
                    }
                    if (total == 0) //if we hit a reflectable surface but are out of bounces... - Nova
                    {
                        Debug.Log("Hit bouce max");
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
                        shootable.OnGettingShot();
                        //we add a node to the line renderer, but we DONT decrease the total, as this isnt a bounce - Nova
                        //we also dont reflect the shot and keep the direction the same - Nova
                    }
                    else //enemies ONLY have shootable - Nova
                    {
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                        shotOrigin = hit.point + shotDirection * 0.01f;
                        Debug.Log("Enemy Hit");
                        Debug.Log(killStreak);
                        playerMovementTutorial.moveSpeed = playerMovementTutorial.moveSpeed + 1f;
                        boostCoolDownStored = playerMovementTutorial.boostCoolDown;
                        Debug.Log($"{boostCoolDownStored}");
                        killStreak = killStreak + 1;
                        shootable.OnGettingShot();
                        enemyNumber = FindObjectsByType<Enemy>(FindObjectsSortMode.None); //reduces the enemy count - Nova
                        Destroy(shootable.GetGameObject()); // this kills the enemy? ig? - Sawyer
                    }
                }

                else //if we hit something that isnt a destroyable or reflectable surface we end the shot;
                {
                    Debug.Log("Non reflect/enemy hit");
                    //Debug.DrawRay(shotOrigin, shotDirection, colors[color], 1000);
                    hitting = false;
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                }
                StartCoroutine(ResetShot());
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
