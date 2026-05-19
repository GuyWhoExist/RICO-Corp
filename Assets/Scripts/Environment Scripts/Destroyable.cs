using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public class Destroyable : MonoBehaviour, IShootable
{
    
    public bool shot;
    public RaycastHit hit;
    [SerializeField] GameObject shardParticles;
    private GameObject particle;
    //this is destroyed on getting shot - Nova
    GameObject IShootable.GetGameObject()
    {
        return gameObject;
    }
    void IShootable.OnGettingShot()
    {
        shot = true;
        particle = Instantiate(shardParticles, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal + new Vector3(0, 180, 0)));
        particle.transform.rotation = Quaternion.Euler(particle.transform.rotation.x + 180, particle.transform.rotation.y, particle.transform.rotation.z);
        Destroy(gameObject);
    }
    public void MeleedA()
    {
        shot = true;
        particle = Instantiate(shardParticles, hit.point + transform.forward, Quaternion.FromToRotation(Vector3.forward, hit.normal + new Vector3(0, 180, 0)));
        particle.transform.rotation = Quaternion.Euler(particle.transform.rotation.x + 180, particle.transform.rotation.y, particle.transform.rotation.z);
        Destroy(gameObject);
    }
    private void OnEnable()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            this.gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
