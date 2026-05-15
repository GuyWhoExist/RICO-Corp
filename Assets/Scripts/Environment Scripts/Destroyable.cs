using UnityEngine;

public class Destroyable : MonoBehaviour, IShootable
{
    public bool shot;
    [SerializeField] GameObject shardParticles;
    //this is destroyed on getting shot - Nova
    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }

    void IShootable.OnGettingShot()
    {
        shot = true;
        Instantiate(shardParticles, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void MeleedA()
    {
        shot = true;
        Instantiate(shardParticles, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnEnable()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            this.gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
