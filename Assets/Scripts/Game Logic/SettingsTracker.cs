using UnityEngine;

public class SettingsTracker : MonoBehaviour
{
    public struct SettingsValues //struct used to store info on Sprays. Gets serialized. - Nova
    {
        [HideInInspector] public float sensitivity;
        [HideInInspector] public float fieldOfView;
        [HideInInspector] public float masterVolume;
        [HideInInspector] public float SFXVolume;
        [HideInInspector] public float musicVolume;
        

        public SettingsValues(float sens, float fov)
        {
            sensitivity = sens;
            fieldOfView = fov;
            masterVolume = 100f;
            SFXVolume = 100f;
            musicVolume = 100f;
        }
    }

    public SettingsValues settings;
    public bool used;

    private void DuplicateRemoval()
    {
        //Debug.Log(used);
        SettingsTracker[] duplicates = FindObjectsByType<SettingsTracker>(FindObjectsSortMode.None);
        Debug.Log(duplicates.Length);
        if (duplicates.Length > 1) //checks for duplicates and destroys them. - Nova
        {
            foreach (SettingsTracker l in duplicates)
            {
                if (l.used == false && duplicates.Length - 1 != 0)
                {
                    Debug.Log("More than 1 settings tracker found, killing the unused ones");
                    //Debug.Log(l.levels[0].bestTime); //data wasnt being loaded. checking if we are deleting 
                    Destroy(l.gameObject);
                }
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject); //allows this object to stay between levels - Nova
        DuplicateRemoval(); //>:( - Nova
    }
}
