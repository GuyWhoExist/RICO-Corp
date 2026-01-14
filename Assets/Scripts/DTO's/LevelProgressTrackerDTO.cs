using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;


public class LevelProgressTrackerDTO
{
    //private LevelProgressTracker levelProgressTracker;
    //private SaveSystem saveSystem;

    public float[] testArray = new float[20];

    //public void ReferenceCarry(LevelProgressTracker levelProgressTracker)
    //{
    //    this.levelProgressTracker = levelProgressTracker;
    //}
    //private void OnEnable()
    //{
    //    levelProgressTracker = FindAnyObjectByType<LevelProgressTracker>();
    //    if (levelProgressTracker != null)
    //    {
    //        Debug.Log("We good in the time controller");
    //    }
    //    else
    //    {
    //        Debug.Log("Things have gone HORRIBLY wrong in the time controller");
    //    }

    //    saveSystem = FindAnyObjectByType<SaveSystem>();
    //    if (saveSystem != null)
    //    {
    //        Debug.Log("We good in the time controller");
    //    }
    //    else
    //    {
    //        Debug.Log("Things have gone HORRIBLY wrong in the time controller");
    //    }
    //    bestTimeConversion();
    //}

//public void bestTimeConversion()
//    {
//        Debug.Log(levelProgressTracker);
//        Debug.Log(levelProgressTracker.levels);
//        for  (int i = 0; i < levelProgressTracker.levels.Length; i++)
//        {
//            testArray [i] = levelProgressTracker.levels[i].bestTime;
//            Debug.Log($"Trackers Time: {levelProgressTracker.levels[i].bestTime}");
//            Debug.Log($"Saved Time: {testArray[i]}");
//        }
//        saveSystem.DTOsave();
//    }


    //public void bestTimeConversionReverted()
    //{
    //    for (int i = 0; i < levelProgressTracker.levels.Length; i++)
    //    {
    //        levelProgressTracker.levels[i].bestTime = testArray[i];
    //        //Debug.Log($"Trackers Time: {levelProgressTracker.levels[i].bestTime}");
    //        //Debug.Log($"Saved Time: {testArray[i]}");
    //    }
    //    Debug.Log("Save Sucessfully Loaded");
    //}
    
   



    //[SerializeField] private object lvlProgScri
    //i want to save the best times fro each level
    //and then wehn it loads i want them to set the best times to the data from here

    //What are the best times?
    // public float best_time = LevelProgressTracker.levels[levelNumber - 1].bestTime();

    //I'm goig to try to get access to this best time, I'm going to try to get it to display in a debug.log
    // private void Update()
    //{
    //    Debug.Log(LevelProgressTracker.levels[levelNumber - 1].bestTime(bestTime);

    //    //



    //}





}