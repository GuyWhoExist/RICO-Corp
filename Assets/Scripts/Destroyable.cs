using UnityEngine;

public class Destroyable : MonoBehaviour, IShootable
{
    public bool shot;
    //this is destroyed on getting shot - Nova
    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }

    void IShootable.OnGettingShot()
    {
        shot = true;
        Destroy(gameObject);
    }
    private void OnEnable()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            this.gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
