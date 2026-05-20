using UnityEngine;

public class ShootToggle : MonoBehaviour
{
    public bool hit;
    [SerializeField] private GameObject target;

    private void Start()
    {
        if (FindFirstObjectByType<PlanningModeController>())
        {
            target.GetComponent<Collider>().isTrigger = true;
        }
    }
    public void RicochetHit()
    {
        if (!hit)
        {
            hit = true;
            target.SetActive(false);
        }
        else
        {
            hit = false;
            target.SetActive(true);
        }
       
    }
}
