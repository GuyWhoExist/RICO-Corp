using System.Collections;
using UnityEngine;

public class SkyTesting : MonoBehaviour
{
    Renderer skyRenderer;
    [SerializeField] float skyChangeTimeValue;
    float sky01Value;
    float sky02Value;

    void Start()
    {
        skyRenderer = GetComponent<Renderer>();
        StartCoroutine(SkyChange());
    }

    IEnumerator SkyChange()
    {
        while (sky02Value < 1)
        {
            sky01Value -= 0.1f / skyChangeTimeValue;
            sky02Value -= 0.1f / skyChangeTimeValue;
            skyRenderer.material.SetFloat("Sky_Amount_1", sky01Value);
            skyRenderer.material.SetFloat("Sky_Amount_2", sky02Value);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
