using UnityEngine;

public class Absorb : MonoBehaviour, IShootable
{
    public bool blasted;
    public RaycastHit hit;
    [SerializeField] private GameObject shardParticles;
    private GameObject particle; 
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
        particle = Instantiate(shardParticles, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        particle.transform.rotation = Quaternion.Euler(particle.transform.rotation.x + 180, particle.transform.rotation.y, particle.transform.rotation.z);
        Destroy(gameObject);
    }
    public void MeleedB()
    {
        blasted = true;
        particle = Instantiate(shardParticles, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal + new Vector3(180, 0, 0)));
        particle.transform.rotation = Quaternion.Euler(particle.transform.rotation.x + 180, particle.transform.rotation.y, particle.transform.rotation.z);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            this.gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
