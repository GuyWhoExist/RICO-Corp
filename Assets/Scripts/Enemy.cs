using UnityEngine;

public class Enemy : MonoBehaviour, IShootable
{
    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }

    void IShootable.OnGettingShot()
    {
        //not needed, i guess? - Nova
    }
    private void OnEnable()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            this.gameObject.GetComponent<Collider>().enabled = false;
    }
}
