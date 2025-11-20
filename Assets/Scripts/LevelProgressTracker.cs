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
        public float milestone2 { get; }
        public float milestone3 { get; }
        public int levelIndex { get; } //stores the level number - Nova

        public float? bestTime; //the "?" after float allows "bestTime" to store a null value - Nova

    }


    private void Awake() //prevents duplicate LevelProgressTrackers (hopefully) - Nova
    {
        used = false;
        LevelProgressTracker[] duplicates = FindObjectsByType<LevelProgressTracker>(FindObjectsSortMode.None);
        if (duplicates.Length > 1)
        {
            foreach (LevelProgressTracker l in duplicates)
            {
                if (l.used == false)
                {
                    Debug.Log("More than 1 tracker found, killing the unused ones");
                    Destroy(l.gameObject);
                }
            }
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public LevelInfo[] levels = new LevelInfo[6] { //the array of levels. - Nova
        new (25f, 20f, 15f, 1 ),
        new (25f, 20f, 15f, 3 ),
        new (25f, 20f, 15f, 4 ),
        new (25f, 20f, 15f, 5 ),
        new (25f, 20f, 15f, 6 ),
        new (25f, 20f, 15f, 7 ),
    };

    public bool used; //used to to track if this is the MAIN tracker and prevents it from being deleted

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
