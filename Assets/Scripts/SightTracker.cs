using UnityEngine;

public class SightTracker : MonoBehaviour
{
    [SerializeField] bool ui;
    public GameObject tracker;
    [SerializeField] GameObject display;
    [SerializeField] GameObject player;
    public bool seen;
    public bool kill;

    private void Awake()
    {
      
    }

    private void Update()
    {
        if (ui == true)
        { 
        display.transform.rotation = tracker.transform.rotation;

        }

        if (ui == false)
        {
            tracker.transform.position = player.transform.position;

        }
    }
}
