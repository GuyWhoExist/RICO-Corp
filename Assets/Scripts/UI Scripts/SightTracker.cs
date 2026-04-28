using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SightTracker : MonoBehaviour
{
    //coded by sawyer
    [SerializeField] bool ui;
    public bool amIDead;
    public GameObject tracker;
    [SerializeField] GameObject display;
    [SerializeField] GameObject player;
    public Material pointerMat;
    [SerializeField] Camera playerCam;
    private Shooting rifleEnemyChecker;
    [HideInInspector] public Vector3 currentThreatPosition;
    [HideInInspector] public RifleEnemy currentThreatObject;
    private bool spotted;
    private RifleEnemy sightCheck;
    private float enemySightRange;
    private Color Purple = new Color32(115, 15, 240, 255);
    private RaycastHit hit;

    private void Start()
    {
        display.SetActive(false);
        UnSpotted();
        rifleEnemyChecker = FindFirstObjectByType<Shooting>();
        sightCheck = FindFirstObjectByType<RifleEnemy>();
        if (sightCheck != null)
        {
            enemySightRange = sightCheck.maxSightDistance;
            InvokeRepeating(nameof(EnemyTrackingCheck), 0.5f, 0.5f);
        }
        sightCheck = null;

        
    }

    private void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z);

            tracker.transform.LookAt(currentThreatPosition);
            display.transform.rotation = tracker.transform.localRotation;
        if (currentThreatObject != null && !amIDead)
        {
            pointerMat.color = Color.Lerp(Purple, Color.red, currentThreatObject.windupTimer);
        }
    }

    public void Spotted()
    {
       display.SetActive(true);
       spotted = true;
       StartCoroutine(EnemyTrackingCheck());

        
    }
    public void UnSpotted()
    {
        display.SetActive(false);
        spotted = false;
        StopCoroutine(EnemyTrackingCheck());
    }


    IEnumerator EnemyTrackingCheck()
    {
        while (spotted == true)
        {
            Physics.Raycast(transform.position, transform.forward, out hit, enemySightRange);
            if (hit.collider == null)
            {
                UnSpotted();
                StopCoroutine(EnemyTrackingCheck());

            }
            else if (!hit.transform.gameObject.GetComponent<RifleEnemy>())
            {
                UnSpotted();
                currentThreatObject = null;
                Debug.Log("there is not an enemy");
                StopCoroutine(EnemyTrackingCheck());
            }
            else
            {
                currentThreatObject = hit.transform.gameObject.GetComponent<RifleEnemy>();
                Debug.Log("there is an enemy");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
