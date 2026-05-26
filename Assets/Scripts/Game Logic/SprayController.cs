using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SprayController : MonoBehaviour
{
    private bool sprays;
    private List<Spray> allSprays;
    private List<Spray> glassSprays = new();
    private List<Spray> armouredGlassSprays = new();
    private int listRoller = 0;
    private void OnEnable()
    {
        Invoke(nameof(SprayFinder), 0.2f);
       
    }

    private void SprayFinder()
    {
       if(FindAnyObjectByType<Spray>() != null)
       {
            sprays = true;
       }
        if (sprays)
        {
            allSprays = new List<Spray>(FindObjectsByType<Spray>(FindObjectsSortMode.None));
            Invoke(nameof(SpraySorter), 0.2f);
        }
    }

    private void SpraySorter()
    {
        Debug.Log(allSprays[listRoller]);
        if (allSprays[listRoller].destroyableObject != null && allSprays[listRoller] != null)
        {
            glassSprays.Add(allSprays[listRoller]);
        }
        else if (allSprays[listRoller].reflectingDestroyableObject != null && allSprays[listRoller] != null)
        {
            armouredGlassSprays.Add(allSprays[listRoller]);
        }
            listRoller++;
        if(listRoller < allSprays.Count)
        {
            SpraySorter();
        }
        else
        {
            listRoller = 0;
        }
    }
    public void GlassImpact()
    {
        if (sprays)
        {
            if (glassSprays.Count > listRoller)
            {
                glassSprays[listRoller].GlassCheck();
                if (glassSprays.Count > listRoller)
                {
                    listRoller++;
                    GlassImpact();
                }
            }
            else
            {
                listRoller = 0;
            }
        }
    }

    public void ArmoredGlassImpact()
    {
        if (sprays)
        {
            if (armouredGlassSprays.Count > listRoller)
            {

                armouredGlassSprays[listRoller].ArmoredGlassCheck();
                if (armouredGlassSprays.Count > listRoller)
                {
                    listRoller++;
                    ArmoredGlassImpact();
                }
            }
            else
            {
                listRoller = 0;
            }
        }
    }
}
