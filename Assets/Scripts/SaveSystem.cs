using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Save();
    }

    public void Save()
    {
        Debug.Log(Application.persistentDataPath);
        //File.WriteAllText(Application.persistentDataPath + "/save.txt", "my data");
        string fileText = File.ReadAllText(Application.persistentDataPath + "/save.txt");
        Debug.Log(fileText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
