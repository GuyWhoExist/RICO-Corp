using NUnit.Framework.Internal;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Config_Internal : MonoBehaviour
{
    [HideInInspector] public float sensitivity;
    [HideInInspector] public float fieldOfView;
    [HideInInspector] public float masterVolume;
    [HideInInspector] public float SFXVolume;
    [HideInInspector] public float musicVolume;
    private void OnEnable()
    {
        DontDestroyOnLoad(transform.gameObject);

        Config_Internal[] duplicates = FindObjectsByType<Config_Internal>(FindObjectsSortMode.None);
        if (duplicates.Length > 1)
        {
            Destroy(gameObject);
        }

    }
}
