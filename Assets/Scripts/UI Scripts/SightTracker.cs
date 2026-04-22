using System;
using UnityEngine;

public class SightTracker : MonoBehaviour
{
    //coded by sawyer
    [SerializeField] bool ui;
    public GameObject tracker;
    [SerializeField] GameObject display;
    [SerializeField] GameObject player;
    [SerializeField] Material pointerMat;
    [SerializeField] Camera playerCam;
    [HideInInspector] public Vector3 currentThreat;
    public bool kill;
    private Color Purple = new Color32(115, 15, 240, 255); 

    private void Start()
    {
        display.SetActive(false);
        UnSpotted();


    }

    private void Update()
    {  
        gameObject.transform.position = player.transform.position;

            tracker.transform.LookAt(currentThreat);
            display.transform.rotation = tracker.transform.localRotation;
    }

    public void InDanger()
    {
        pointerMat.color = Color.red;
    }
    public void InSafe()
    {
        pointerMat.color = Purple;
    }

    public void Spotted()
    {
            display.SetActive(true);
    }
    public void UnSpotted()
    {
        display.SetActive(false);
    }
}
