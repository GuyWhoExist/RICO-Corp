using UnityEngine;

public class Absorb : MonoBehaviour, IShootable
{
    //i GENUINELY cant remember what this does, but i think its needed? Probably not. - Nova

    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }

    void IShootable.OnGettingShot()
    {
        Destroy(gameObject);
    }
}
