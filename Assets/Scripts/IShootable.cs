using UnityEngine;

public interface IShootable 
{
    //for shootable things (enemies, glass, etc.) - Nova
    void OnGettingShot();
    GameObject GetGameObject();

}
