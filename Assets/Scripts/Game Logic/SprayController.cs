using System.Collections.Generic;
using UnityEngine;

public class SprayController : MonoBehaviour
{
    private List<Spray> allSprays;
    private int listRoller = 0;
    private void OnEnable()
    {
        Invoke(nameof(SprayFinder), 0.5f);
       
    }

    private void SprayFinder()
    {
        allSprays = new List<Spray>(FindObjectsByType<Spray>(FindObjectsSortMode.None));
    }

    
    public void GlassImpact()
    {
        if (allSprays.Count > 0 && allSprays.Count > listRoller)
        {
            allSprays[listRoller].GlassCheck();
            if (allSprays.Count > listRoller)
            {
                listRoller += 1;
                GlassImpact();
            }
            else
            {
                listRoller = 0;
            }

        }
      
    }

    public void ArmoredGlassImpact()
    {
        if (allSprays.Count > 0 && allSprays.Count > listRoller)
        {
            allSprays[listRoller].ArmoredGlassCheck();
            if (allSprays.Count > listRoller)
            {
                listRoller += 1;
                ArmoredGlassImpact();
            }
            else
            {
                listRoller = 0;
            }
        }
       
    }
}
