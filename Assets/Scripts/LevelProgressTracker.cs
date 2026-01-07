using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
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
        public int levelIndex { get; } //stores the level number, kinda redundant - Nova

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
                    Debug.Log(l.levels[0].bestTime);
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

    public LevelInfo[] levels = new LevelInfo[9] { //the array of levels. - Nova
        new (20f, 15f, 10f, 2 ),
        new (8f, 4f, 2f, 3 ),
        new (15f, 10f, 5f, 4 ),
        new (25f, 20f, 15f, 5 ),
        new (30f, 20f, 15f, 6 ),
        new (25f, 20f, 15f, 7 ),
        new (21f, 14f, 7f, 8 ),
<<<<<<< HEAD
        new (120f, 75f, 45f, 9),
        new (120f, 75f, 45f, 12) //Level 1 v2 - Blockout Colby, Archetect tbd
=======
        new (200f, 115f, 45f, 9)

>>>>>>> c2c46b94c977a853a2eeb9091f64e9a1cf0ed011
    };

    public bool used; //used to to track if this is the MAIN tracker and prevents it from being deleted - Nova
    public float testingTime;

    public int GetArrayIndex( int levelIndex ) //i realized this was redundant a few hours after i coded this. so yeah... - Nova
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


}
