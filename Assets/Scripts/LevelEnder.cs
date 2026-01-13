using UnityEngine;

public class LevelEnder : MonoBehaviour
{
    //this object ends the level (duh) - Nova

    [SerializeField] float bonus;
    [SerializeField] public int nextLevelIndex;
    [SerializeField] public bool secret;
    
    public float GetBonus()
    {
        return bonus;
    }

    public int GetNextIndex()
    {
        return nextLevelIndex;
    }

    private void OnEnable()
    {
        if (FindAnyObjectByType<PlanningModeController>())
            this.gameObject.GetComponent<Collider>().enabled = false;
    }
}
