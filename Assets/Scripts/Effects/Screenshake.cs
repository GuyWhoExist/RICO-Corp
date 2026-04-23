using UnityEngine;

public class Screenshake : MonoBehaviour
{
    //used to apply screenshake during a speed boost
    //Written by Nova

    [Header("Shake Info")]
    [SerializeField] float baseIntensity; //how intense the screenshake is - Nova
    [SerializeField] float decreaseFactor; //how quickly the intensity decreases
    [SerializeField] float increaseFactor; //how quickly the intensity increases

    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] SpeedBoost speedBoost;
    private void Awake()
    {
        InvokeRepeating(nameof(ScreenShaker), 0.05f, 0.05f);
    }
    private void ScreenShaker()
    {
        float intensity = 0;

        if (speedBoost.boostRemaining >= speedBoost.boostDuration * 2)
        {
            intensity = baseIntensity * 3;
            Debug.Log("Max Intensity");
        }
        else if (speedBoost.boostRemaining > speedBoost.boostDuration)
        {
            intensity = baseIntensity * 2;
            Debug.Log("2x Intensity");
        }
        else if (speedBoost.boostRemaining > 0)
        {
            intensity = baseIntensity;
            Debug.Log("1x Intensity");
        }
        else
        {
            intensity = 0;
        }

        if (speedBoost.boostRemaining > 0)
        {
            Vector3 pos = Random.insideUnitCircle * intensity;
            pos.y += 0.5f;
            cam.transform.localPosition = pos;
            
        }
        else
        {
            cam.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            Debug.Log("Shake stopped");
            Debug.Log(speedBoost.boostRemaining);
        }
    }
}
