using UnityEngine;

public class SettingsTracker : MonoBehaviour
{
    public struct SettingsValues //struct used to store info on Sprays. Gets serialized. - Nova
    {
        public float sensitivity;
        public float FOV;
       

        public SettingsValues(float sens, float fov)
        {
            sensitivity = sens;
            FOV = fov;
        }
    }

    public SettingsValues settingsValues;
}
