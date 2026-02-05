using UnityEngine;

public class SightTracker : MonoBehaviour
{
    [SerializeField] bool ui;
    public GameObject tracker;
    [SerializeField] GameObject display;
    [SerializeField] GameObject player;
    [SerializeField] Camera playerCam;
    [HideInInspector] public Vector3 currentThreat;
    public bool seen;
    public bool kill;

    private void Update()
    {

        this.transform.position = player.transform.position;
        if (seen)
        {
            tracker.transform.LookAt(currentThreat);
            display.transform.rotation = tracker.transform.rotation;
        }
        else
        {
            this.transform.rotation = playerCam.transform.rotation;
            display.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
}
