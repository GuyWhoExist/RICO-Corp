using UnityEngine;

public class Absorb : MonoBehaviour, IShootable
{
    public bool blasted;
    [SerializeField] private GameObject shardParticles;
    //private GameObject particleObject;
    //i GENUINELY cant remember what this does, but i think its needed? Probably not. - Nova
    //it is needed, this is used to differentiate armoured glass. - Sawyer

    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }

    void IShootable.OnGettingShot()
    {
        blasted = true;
        Instantiate(shardParticles, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void MeleedB()
    {
        blasted = true;
        Instantiate(shardParticles, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            this.gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
