using UnityEngine;

public class FireFlickering : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private float baseIntensity;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    private float timer;
    private float randLimit;


    private void Update()
    {
        if (light.intensity <= baseIntensity || timer >= randLimit)
        {
            light.intensity = Random.Range(minIntensity, maxIntensity);
            timer = 0;
            randLimit = Random.Range(0.1f, 0.5f);
        }
        else
        {
            light.intensity -= (Time.deltaTime*5);
            timer += Time.deltaTime;
        }
    }
}
