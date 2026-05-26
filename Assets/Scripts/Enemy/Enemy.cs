using UnityEngine;

public class Enemy : MonoBehaviour, IShootable
{

    public bool shotImmune; //makes the enemy unable to be killed from shots - Nova
    public bool meleeImmune; //makes the enemy unable to be killed from melee - Nova
    public bool bounceImmune; //makes the enemy unable to be killed from melee bounce (still provides the boost) - Nova
    public bool shot; //prevents the enemy from giving boost when being directly shot in specific situations - Nova

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
