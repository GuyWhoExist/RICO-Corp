using UnityEngine;

public class LevelEnder : MonoBehaviour
{
    //this object ends the level (duh) - Nova

    [SerializeField] float bonus;
    [SerializeField] public int nextLevelIndex;
    
    public float GetBonus()
    {
        return bonus;
    }

    public int GetNextIndex()
    {
        return nextLevelIndex;
    }

}
