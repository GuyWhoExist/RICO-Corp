using UnityEngine;

public class Enemy : MonoBehaviour, IShootable
{

    public bool shotImmune;
    public bool meleeImmune;
    public bool bounceImmune;

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
