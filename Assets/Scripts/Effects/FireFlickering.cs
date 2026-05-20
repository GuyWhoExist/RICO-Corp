using UnityEngine;

public class FireFlickering : MonoBehaviour
{
    [SerializeField] private Light lightObject;
    [SerializeField] private float baseIntensity;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    private float timer;
    private float randLimit;


    private void Update()
    {
        if (lightObject.intensity <= baseIntensity || timer >= randLimit)
        {
            lightObject.intensity = Random.Range(minIntensity, maxIntensity);
            timer = 0;
            randLimit = Random.Range(0.1f, 0.5f);
        }
        else
        {
            lightObject.intensity -= (Time.deltaTime*5);
            timer += Time.deltaTime;
        }
    }
}
