using UnityEngine;

public class PlanningModeController : MonoBehaviour
{
    public bool planning;
    private bool planState;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        //Debug.Log("planning mode enabled");
    }
}
