using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelProgressTracker : MonoBehaviour
{
    [HideInInspector] public bool levelCompleted;
    [HideInInspector] public bool levelLoaded;
    private bool checkComplete;
    private float valueCheckDelay;
    private LevelEnder levelEnder;
    [HideInInspector] public PauseMenu pauseMenu;
    [HideInInspector] public TimerController timerController;

    //Contains all level data
    //

    private void OnEnable()
    {
        checkComplete = true;

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
    public void LevelStatusCheck()
    {
            valueCheckDelay += Time.deltaTime;
            if (valueCheckDelay > 0.01)
            {
                if (levels[levelEnder.nextLevelIndex - 3].bestTime == -1f)
                {
                    levelCompleted = false;
                    Debug.Log($"disabling locked features, because level {levelEnder.nextLevelIndex - 3} besttime is : {levels[levelEnder.nextLevelIndex - 3].bestTime}");
                    valueCheckDelay = 0f;
                    checkComplete = true;
                    pauseMenu.completionCheck = false;
                    timerController.statusCheck = false;
                }
                else
                {
                    levelCompleted = true;
                    Debug.Log($"Enabling locked features, because  level {levelEnder.nextLevelIndex - 3} besttime is : {levels[levelEnder.nextLevelIndex - 3].bestTime}");
                    valueCheckDelay = 0f;
                    checkComplete = true;
                    pauseMenu.completionCheck = false;
                    timerController.statusCheck = false;
                }
            }
            Debug.Log(levelCompleted);
    }
    private void Update()
    {
        //checks for loading of new level and prevents it from checking more then once per level - sawyer
        if (levelLoaded == true)
        {
            levelEnder = null;
            Debug.Log("firing check");
            if (FindFirstObjectByType(typeof(LevelEnder)) != null)
            {
                if (FindAnyObjectByType<PauseMenu>())
                    pauseMenu = FindAnyObjectByType<PauseMenu>();
                if (FindAnyObjectByType<TimerController>())
                    timerController = FindAnyObjectByType<TimerController>();
                levelEnder = FindFirstObjectByType<LevelEnder>();
                levelLoaded = false;
                checkComplete = false;
                Debug.Log("LevelEnder found");
            }
            else
            {
                if (FindAnyObjectByType<PauseMenu>())
                    pauseMenu = FindAnyObjectByType<PauseMenu>();
                if (FindAnyObjectByType<TimerController>())
                    timerController = FindAnyObjectByType<TimerController>();
                Debug.Log("LevelEnder not found");
                levelLoaded = false;
            }
        }
        if (checkComplete == false)
        {
            LevelStatusCheck();
        }
        // end
        testingTime = levels[0].bestTime;
        LevelProgressTracker[] duplicates = FindObjectsByType<LevelProgressTracker>(FindObjectsSortMode.None);
        if (duplicates.Length > 1 && used != false) //checks for duplicates and destroys them. - Nova
        {
            foreach (LevelProgressTracker l in duplicates)
            {
                if (l.used == false && duplicates.Length - 1 != 0)
                {
                    Debug.Log("More than 1 tracker found, killing the unused ones");
                    Debug.Log(l.levels[0].bestTime); //data wasnt being loaded. checking if we are deleting 
                    Destroy(l.gameObject);
                }
            }
        }
    }

    private void Awake()
    {
        used = false;
        DontDestroyOnLoad(transform.gameObject); //allows this object to stay between levels - Nova
    }

    public LevelInfo[] levels = new LevelInfo[8] { //the array of levels. - Nova
        //star 1, star 2, star 3, scene index
        new (20f, 15f, 10f, 2 ),
        new (8f, 4f, 2f, 3 ),
        new (15f, 10f, 5f, 4 ),
        new (25f, 20f, 15f, 5 ),
        new (30f, 20f, 15f, 6 ),
        new (25f, 20f, 15f, 7 ),
        new (21f, 14f, 7f, 8 ),
        new (200f, 115f, 45f, 9), //who set this? 75 is not a valid time lol. 75 would be 0:75.00, which is not possible. - Nova
       // new (120f, 75f, 45f, 10), //Level 1 v2 - Blockout Colby, Archetect tbd
       //new (200f, 115f, 45f, 9),
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


    public LevelProgressTrackerDTO GetDTO()
   
        //public void bestTimeConversion()
    {
        float[] testArray = new float[20];

        //Debug.Log(levelProgressTracker);
        //Debug.Log(levelProgressTracker.levels);
        for (int i = 0; i < levels.Length; i++)
        {
            testArray[i] = levels[i].bestTime;
            Debug.Log($"Trackers Time: {levels[i].bestTime}");
            Debug.Log($"Saved Time: {testArray[i]}");
        }
        LevelProgressTrackerDTO newlevelProgTrockDTO = new LevelProgressTrackerDTO();
        newlevelProgTrockDTO.testArray = testArray;

        return newlevelProgTrockDTO;
    }

    public void LoadMethod(float[] revertArray)
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
