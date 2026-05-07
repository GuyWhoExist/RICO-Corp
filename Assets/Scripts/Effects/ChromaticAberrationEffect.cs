using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationEffect : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private ChromaticAberration cA;

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
        cA.SetAllOverridesTo(true);
    }

    public void Activate()
    {
        Debug.Log("CA Fired");

        cA.intensity.value = 1f;

        while (cA.intensity.value > 0)
        {
           cA.intensity.value -= Time.deltaTime*0.01f;
        }
        Debug.Log($"CA Ended {cA.intensity.value}");
    }

    
}
