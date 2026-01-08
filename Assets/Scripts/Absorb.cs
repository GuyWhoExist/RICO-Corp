using UnityEngine;

public class Absorb : MonoBehaviour, IShootable
{
    public bool blasted;
    //i GENUINELY cant remember what this does, but i think its needed? Probably not. - Nova

    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }

    void IShootable.OnGettingShot()
    {
        blasted = true;
        Destroy(gameObject);
    }
}
