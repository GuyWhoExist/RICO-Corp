using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using static LevelProgressTrackerDTO;

public class SaveSystem : MonoBehaviour
{
    //Save system for the game - Nova
    //Written by Colby and Nova

    private string filePath;
    // private LevelProgressTrackerDTO levelProgressTrackerDTO;
    [SerializeField] private LevelProgressTracker levelProgressTracker;
    private float[] revertArrayArray = new float[20];
    public List<Spray>[] importedSprays = new List<Spray>[10];
    private SaveSystem[] test;
    [SerializeField] private PersistentObject pO;



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

        test = FindObjectsByType<SaveSystem>(FindObjectsSortMode.None);
        filePath = Application.persistentDataPath + "/save.json";
        if (test.Length == 1)
        {
            //Debug.Log("one pO found, setting lpt to used and reloading scene");
            levelProgressTracker.used = true;
            //Debug.Log(levelProgressTracker.used);
            pO.used = true;
            DTOload();
            //Debug.Log(levelProgressTracker.sprays[0].Count);
        }
       
        

        
        

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


    public void DTOsave() //saves all data into a JSON to be used in future runtimes - Nova
    {
        LevelProgressTrackerDTO levelProgressTrackerDTO = levelProgressTracker.GetDTO();

        // DTO -> string
        string savedJson = JsonConvert.SerializeObject
        (
            levelProgressTrackerDTO,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, //prevents an error caused by a recursive normalization loop - Nova 
                Formatting = Formatting.Indented, //makes the JSON more readable - Nova
            }
        ); 

        // string -> file

        File.WriteAllText(filePath, savedJson);

        Debug.Log("Sucessfully Saved");

        Debug.Log(filePath);
    }


    public void DTOload() //loads data from the JSON file - Nova
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

        if (DTO.sprayArray == null) //puts the DTO sprayArray data into the LPT spray data - Nova
        {
            for (int i = 0; i < DTO.sprayArray.Length; i++)
            {
                DTO.sprayArray[i] = new List<SprayInfo>();
            }
        }

        //tell level progress tracker to run it's load method and take this dto as an argumemnt so it can use it's array
        revertArrayArray = DTO.timeArray;
        levelProgressTracker.sprays = DTO.sprayArray;
        Debug.Log("Updated sprays");
        //if (DTO.sprayArray != null)
        //{
        //    for (int x = 0; x < DTO.sprayArray.Length; x++)
        //    {
        //        if (DTO.sprayArray[x] != null)
        //        {
        //            for (int y = 0; y < DTO.sprayArray[x].Count; y++)
        //            {
        //                levelProgressTracker.sprays[x][y] = DTO.sprayArray[x][y];
        //            }
        //        }

        //        //for (int y = 0; y < DTO.sprayArray[x].Count; y++)
        //        //{
        //           // levelProgressTracker.sprays[x][y] = DTO.sprayArray[x][y];
        //       // }
        //    }
        //}
        levelProgressTracker.used = true;
        levelProgressTracker.LoadMethod(revertArrayArray);
        

        Debug.Log("Sucessful Load");

        
        // info you need to be able to do the thing (here ima help you get set up)
      //  DTO.ReferenceCarry(levelProgressTracker);

        // what you really wanna do
      //  levelProgressTrackerDTO.bestTimeConversionReverted();
    }
}
