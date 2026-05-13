using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationEffect : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private ChromaticAberration cA;
    private LensDistortion lD;
    [SerializeField] private float lensDistortionAmount;

    private void Awake()
    {
        

        if (volume.profile.TryGet<ChromaticAberration>(out cA))
        {
            Debug.Log("Chromatic Aberration found");
        }
        else
        {
            Debug.Log("Chromatic Aberration missing");
        }
        
        if (volume.profile.TryGet<LensDistortion>(out lD))
        {
            Debug.Log("Lens Distortion found");
        }
        else
        {
            Debug.Log("Lens Distortion missing");
        }

        cA.SetAllOverridesTo(true);
        lD.SetAllOverridesTo(true);
    }

    private void Update()
    {
        if (cA.intensity.value > 0)
        {
            cA.intensity.value -= Time.deltaTime;
        }
        if (lD.intensity.value > 0)
        {
            lD.intensity.value -= Time.deltaTime *1.5f;
        }

        if (cA.intensity.value < 0)
        {
            cA.intensity.value = 0;
        }
        if (lD.intensity.value < 0)
        {
            lD.intensity.value = 0;
        }
    }

    public void Activate()
    {
        Debug.Log("CA Fired");

        cA.intensity.value = 1f;
        lD.intensity.value = lensDistortionAmount;

    }

    
}
