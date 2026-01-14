using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PersistentObject : MonoBehaviour
{
    public bool used; //used to to track if this is the MAIN tracker and prevents it from being deleted - Nova


    // A static instance to reference this object globally
    public static PersistentObject Instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // Set this as the current instance
        Instance = this;

        // Make this object persist across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        PersistentObject[] duplicates = FindObjectsByType<PersistentObject>(FindObjectsSortMode.None);
        if (duplicates.Length > 1 && used != false) //checks for duplicates and destroys them. - Nova
        {
            foreach (PersistentObject l in duplicates)
            {
                if (l.used == false && duplicates.Length - 1 != 0)
                {
                    Debug.Log("More than 1 persistent object found, killing the unused ones");
                   
                    Destroy(l.gameObject);
                }
            }
        }
    }
}


