using System;
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
    [HideInInspector] public Vector3 currentThreatPosition;
    [HideInInspector] public RifleEnemy currentThreatObject;
    public bool kill;
    private RifleEnemy sightCheck;
    private float enemySightRange;
    private Color Purple = new Color32(115, 15, 240, 255);

    private void Start()
    {
        display.SetActive(false);
        UnSpotted();
        sightCheck = FindFirstObjectByType<RifleEnemy>();
        enemySightRange = sightCheck.maxSightDistance;
        sightCheck = null;
        InvokeRepeating(nameof(EnemyTrackingCheck), 0.5f, 0.5f);

    }

    private void Update()
    {  
        gameObject.transform.position = player.transform.position;

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
            EnemyTrackingCheck();
            
    }
    public void UnSpotted()
    {
        display.SetActive(false);
    }


    private void EnemyTrackingCheck()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit enemy, enemySightRange);
        if (!enemy.transform.GetComponent<RifleEnemy>())
        {
            UnSpotted();
            currentThreatObject = null;
        }
        else
        {
            currentThreatObject = enemy.transform.GetComponent<RifleEnemy>();
        }
    }
}
