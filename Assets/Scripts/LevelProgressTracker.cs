using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressTracker : MonoBehaviour
{

    //Contains all level data
    //
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

    private void Update()
    {
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
