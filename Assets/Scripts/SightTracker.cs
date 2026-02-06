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
    private bool UIVisible;
    public bool seen;
    public bool kill;
    public bool danger;
    private Color Purple = new Color32(115, 15, 240, 255);

    private void Awake()
    {
        display.SetActive(false);
        UIVisible = false;
    }
    private void Update()
    {

        this.transform.position = player.transform.position;
        if (seen)
        {
            if (UIVisible == false)
            {
                display.SetActive(true);
                UIVisible = true;
            }
            tracker.transform.LookAt(currentThreat);
            display.transform.rotation = tracker.transform.localRotation;
            if (danger == true)
            {
               pointerMat.color = Color.red;
            }
            else
            {
                pointerMat.color = Purple;
            }
        }
        else
        {
            this.transform.rotation = playerCam.transform.rotation;
            display.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (UIVisible == true)
            {
                display.SetActive(false);
                UIVisible = false;
            }
        }

    }
}
