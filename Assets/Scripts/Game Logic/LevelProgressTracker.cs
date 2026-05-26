using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static LevelProgressTrackerDTO;

public class LevelProgressTracker : MonoBehaviour
{
    [HideInInspector] public bool levelCompleted;
    private bool checkComplete;
    private float valueCheckDelay;
    private LevelEnder levelEnder;
    [HideInInspector] public PauseMenu pauseMenu;
    [HideInInspector] public TimerController timerController;
    [HideInInspector] public bool initialComplete;
    public float bestTimeStored;
    private Cheats cheats;
    public bool cheatsEnemyCountStatus;
    [SerializeField] private GameObject shootMarker; // type 1
    [SerializeField] private GameObject stopMarker; // type 2
    [SerializeField] private GameObject goMarker; // type 3
    //Contains all level data
    //written by Nova

    

    private void OnEnable()
    {
        checkComplete = true;

            cheats = FindAnyObjectByType<Cheats>();
            if (cheats.enemyCounter == true)
            {
                cheatsEnemyCountStatus = true;
            }
        
    }

    public struct LevelInfo
    {
        public LevelInfo(float m1, float m2, float m3, int index)
        {
            milestone1 = m1;
            milestone2 = m2;
            milestone3 = m3;
            levelIndex = index;
            bestTime = -1f;
        }

        public float milestone1 { get; } //time milestones. hitting m1 would be the minimum. - Nova

        //milestones and time as a whole for curTime and other things are stored in 0:00.00 format, EVEN IN THE CODE. (Ex. If Milestone 3 was 125f, that would be 1:25.00)

        public float milestone2 { get; }
        public float milestone3 { get; }
        public int levelIndex { get; } //stores the level number, kinda redundant, but dont remove. - Nova

        public float bestTime; //the "?" after float allows "bestTime" to store a null value - Nova
        
    }



    //LevelStatusCheck coded by sawyer
    public void LevelStatusCheck()
    {
            valueCheckDelay += Time.deltaTime;
        if (valueCheckDelay > 0.01)
        {

            if (levelEnder.nextLevelIndex == 0)
            {
                if (levels[levels.Length-1].bestTime == -1f)
                {
                    levelCompleted = false;
                    Debug.Log($"disabling locked features, because level {levels.Length - 1} besttime is : {levels[levels.Length - 1].bestTime}");
                    valueCheckDelay = 0f;
                    checkComplete = true;
                    pauseMenu.CompletionCheck();
                    timerController.statusCheck = false;
                    bestTimeStored = -1;
                }
                else
                {
                    levelCompleted = true;
                    Debug.Log($"Enabling locked features, because  level {levels.Length - 1} besttime is : {levels[levels.Length - 1].bestTime}");
                    valueCheckDelay = 0f;
                    checkComplete = true;
                    pauseMenu.CompletionCheck();
                    timerController.statusCheck = false;
                    initialComplete = false;
                    bestTimeStored = levels[levels.Length - 1].bestTime;


                }
            }
            else
            {
                if (levels[levelEnder.nextLevelIndex - 3].bestTime == -1f)
                {
                    levelCompleted = false;
                    Debug.Log($"disabling locked features, because level {levelEnder.nextLevelIndex - 3} besttime is : {levels[levelEnder.nextLevelIndex - 3].bestTime}");
                    valueCheckDelay = 0f;
                    checkComplete = true;
                    pauseMenu.CompletionCheck();
                    timerController.statusCheck = false;
                    bestTimeStored = -1;
                }
                else
                {
                    levelCompleted = true;
                    Debug.Log($"Enabling locked features, because  level {levelEnder.nextLevelIndex - 3} besttime is : {levels[levelEnder.nextLevelIndex - 3].bestTime}");
                    valueCheckDelay = 0f;
                    checkComplete = true;
                    pauseMenu.CompletionCheck();
                    timerController.statusCheck = false;
                    initialComplete = false;
                    bestTimeStored = levels[levelEnder.nextLevelIndex - 3].bestTime;


                }
            }
        }
            Debug.Log(levelCompleted);
    }


    public void LevelLoaded()
    {
        //checks for loading of new level and prevents it from checking more then once per level - sawyer
        {
            levelEnder = null;
            Debug.Log("firing check");
            if (FindFirstObjectByType(typeof(LevelEnder)) != null)
            {
                    pauseMenu = FindAnyObjectByType<PauseMenu>();
                    timerController = FindAnyObjectByType<TimerController>();
                levelEnder = FindFirstObjectByType<LevelEnder>();
                checkComplete = false;
                initialComplete = true;
            }

            if (checkComplete == false)
            {
                LevelStatusCheck();
            }
        }
        // end
    }

    private void DuplicateRemoval()
    {
        //Debug.Log(used);
        testingTime = levels[0].bestTime;
        LevelProgressTracker[] duplicates = FindObjectsByType<LevelProgressTracker>(FindObjectsSortMode.None);
        if (duplicates.Length > 1) //checks for duplicates and destroys them. - Nova
        {
            foreach (LevelProgressTracker l in duplicates)
            {
                if (l.used == false && duplicates.Length - 1 != 0)
                {
                    Debug.Log("More than 1 tracker found, killing the unused ones");
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
        //for (int i = 0; i < sprays.Length; i++)
        //{
        //    sprays[i] = new List<LevelProgressTrackerDTO.SprayInfo>();
        //}
    }

    public LevelInfo[] levels = new LevelInfo[10] { //the array of levels. - Nova
        //star 1, star 2, star 3, scene index
        new (45f, 35f, 18f, 2 ), //tutorial 1 (1)
        new (32f, 22f, 12f, 3 ), //tutorial 2 (2)
        new (20f, 12f, 8f, 4 ), //level 1 (3)
        new (12f, 8f, 5f, 5 ), //level 1.1 (4)
        new (18f, 12f, 7f, 6 ), //level 2.1 (5)
        new (24f, 18f, 12f, 7 ), //level 4 (6)
        new (35f, 24f, 18f, 8 ), //level 5 (7)
        new (25f, 20f, 15f, 9 ), //level 6 (8)
        new (21f, 14f, 9f, 10 ), //level 7 (9) 
        new (110f, 50f, 40f, 11), //alleyway (10)
        
       // new (120f, 75f, 45f, 10), //Level 1 v2 - Blockout Colby, Archetect tbd
       //new (200f, 115f, 45f, 9),
    };

    public List<SprayInfo>[] sprays = new List<SprayInfo>[10] //first [] = Level -- second [] = spray number - Nova
    {
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>(),
    new List<SprayInfo>()
    };







    public bool used; //used to to track if this is the MAIN tracker and prevents it from being deleted - Nova
    public float testingTime; //debugging field. used to check if this LPT has valid data - Nova

    public int GetArrayIndex( int levelIndex ) //i realized this was redundant a few hours after i coded this. This doesn't get used anywhere, but leave it. - Nova
    {
        int arrayIndex = 0;
        foreach ( LevelInfo level in levels )
        {
            if ( level.levelIndex == levelIndex )
            {
                return arrayIndex;
            }
            else
            {
                arrayIndex++;
            }
        }
        return -1;
    }

    public void LoadSprays(int location) //loads/spawns sprays - Nova
    {
        int count = 0;
        //Debug.Log(testingTime);
        Debug.Log($"Load started for location {location}");
        Debug.Log($"Number of sprays saved in {location}: {sprays[location].Count}");
        for (int i = 0; i < sprays[location].Count; i++)
        {
            if (sprays[location][i].type == 1)
            {
                GameObject sprayInstance = GameObject.Instantiate(shootMarker, sprays[location][i].position, sprays[location][i].rotation) as GameObject;
                sprayInstance.GetComponent<Spray>().destructible = sprays[location][i].destroy;
                Debug.Log($"Loaded spray {i} of type Shoot");
            }
            else if (sprays[location][i].type == 2)
            {
                GameObject sprayInstance = GameObject.Instantiate(stopMarker, sprays[location][i].position, sprays[location][i].rotation) as GameObject;
                sprayInstance.GetComponent<Spray>().destructible = sprays[location][i].destroy;
                Debug.Log($"Loaded spray {i} of type Stop");
            }
            else if (sprays[location][i].type == 3)
            {
                GameObject sprayInstance = GameObject.Instantiate(goMarker, sprays[location][i].position, sprays[location][i].rotation) as GameObject;
                sprayInstance.GetComponent<Spray>().destructible = sprays[location][i].destroy;
                Debug.Log($"Loaded spray {i} of type Go");
            }
            count++;
        }
        Debug.Log($"List checked contents:"); //this section is used for debugging what we spawned - Nova
        for (int i = 0; i < sprays[location].Count; i++)
        {
            Debug.Log("the loop is happening at least");
            Debug.Log($"Number: {i}  Position: {sprays[location][i].position}  Rotation: {sprays[location][i].rotation}  Type:  {sprays[location][i].type}");
        }
        Debug.Log($"Loaded {count} sprays");
    }



    public LevelProgressTrackerDTO GetDTO()
   
        //public void bestTimeConversion()
    {
        float[] testArray = new float[20];

        //Debug.Log(levelProgressTracker);
        //Debug.Log(levelProgressTracker.levels);
        for (int i = 0; i < levels.Length; i++)
        {
            testArray[i] = levels[i].bestTime;
            //Debug.Log($"Trackers Time: {levels[i].bestTime}");
            //Debug.Log($"Saved Time: {testArray[i]}");
        }

        
        Vector3 savingPosition = Vector3.zero;
        Vector3 savingRotation = Vector3.zero;

        LevelProgressTrackerDTO newlevelProgTrockDTO = new LevelProgressTrackerDTO();
        newlevelProgTrockDTO.timeArray = testArray;
        
        for (int x = 0; x < newlevelProgTrockDTO.sprayArray.Length; x++) //puts the current sprays into the DTO - Nova
        {
            for (int y = 0; y < sprays[x].Count; y ++)
            {
                savingPosition = sprays[x][y].position;
                savingRotation.x = sprays[x][y].rotation.x;
                savingRotation.y = sprays[x][y].rotation.y;
                savingRotation.z = sprays[x][y].rotation.z;

                newlevelProgTrockDTO.sprayArray[x].Add(sprays[x][y]);
            }
        }
        return newlevelProgTrockDTO;
    }

    

    public void LoadMethod(float[] revertArray) //loads the times - Nova
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].bestTime = revertArray[i];
            //Debug.Log($"Trackers Time: {levelProgressTracker.levels[i].bestTime}");
            //Debug.Log($"Saved Time: {testArray[i]}");
        }
        Debug.Log("Save Sucessfully Loaded");
        used = true;
        SceneManager.LoadScene(0);
    }

}
