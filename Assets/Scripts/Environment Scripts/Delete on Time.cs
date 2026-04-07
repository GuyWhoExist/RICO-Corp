using UnityEngine;

public class DeleteonTime : MonoBehaviour
{
    [SerializeField] private float timeToRemove;
    [Header("particles field is optional")]
    [SerializeField] ParticleSystem particles;

    void Update()
    {
        timeToRemove -= Time.deltaTime;
        if (timeToRemove < 0)
        {
            if (particles != null)
            {
                Destroy(particles);
            }
            else
            {
                Destroy(this.gameObject);
            }

        }
    }
}
