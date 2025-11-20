using UnityEngine;

public class Destroyable : MonoBehaviour, IShootable
{

    //this is destroyed on getting shot - Nova
    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }

    void IShootable.OnGettingShot()
    {
        Destroy(gameObject);
    }
}
