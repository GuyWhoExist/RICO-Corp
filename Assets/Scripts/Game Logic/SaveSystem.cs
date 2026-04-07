using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    private string filePath;
    // private LevelProgressTrackerDTO levelProgressTrackerDTO;
    [SerializeField] private LevelProgressTracker levelProgressTracker;
    private float[] revertArrayArray = new float[20];



    private void Awake()
    {

        //levelProgressTrackerDTO = FindAnyObjectByType<LevelProgressTrackerDTO>();
        //if (levelProgressTrackerDTO != null)
        //{
        //    Debug.Log("We good in the SaveSystem");
        //}
        //else
        //{
        //    Debug.Log("Things have gone HORRIBLY wrong in the SaveSystem");
        //}


        filePath = Application.persistentDataPath + "/save.json";


    }

    //public void bestTimeConversion()
    //{
    //    float[] testArray = new float[20];



    //    Debug.Log(levelProgressTracker);
    //    Debug.Log(levelProgressTracker.levels);
    //    for (int i = 0; i < levelProgressTracker.levels.Length; i++)
    //    {
    //        testArray[i] = levelProgressTracker.levels[i].bestTime;
    //        Debug.Log($"Trackers Time: {levelProgressTracker.levels[i].bestTime}");
    //        Debug.Log($"Saved Time: {testArray[i]}");
    //    }
    //    saveSystem.DTOsave();
    //}


    public void DTOsave()
    {
        LevelProgressTrackerDTO levelProgressTrackerDTO = levelProgressTracker.GetDTO();
        // DTO -> string (ser
        string savedJson = JsonConvert.SerializeObject(levelProgressTrackerDTO); //I took settings out

        // string -> file
        File.WriteAllText(filePath, savedJson);

        Debug.Log("Sucessfully Saved");

        Debug.Log(filePath);
    }


    public void DTOload()
    {
        if (levelProgressTracker != null)
        {
            Debug.Log("LvlProgTracker good");
        }
        Debug.Log("LOAD sTARTED");

        // file -> string
        string loadedJson = File.ReadAllText(filePath);

        // string ->  DTO
        LevelProgressTrackerDTO DTO = JsonConvert.DeserializeObject<LevelProgressTrackerDTO>(loadedJson);

        //tell level progress tracker to run it's load method and take this dto as an argumemnt so it can use it's array
        revertArrayArray = DTO.testArray;
        

        levelProgressTracker.LoadMethod(revertArrayArray);

        Debug.Log("Sucessful Load");

        
        // info you need to be able to do the thing (here ima help you get set up)
      //  DTO.ReferenceCarry(levelProgressTracker);

        // what you really wanna do
      //  levelProgressTrackerDTO.bestTimeConversionReverted();
    }
}
