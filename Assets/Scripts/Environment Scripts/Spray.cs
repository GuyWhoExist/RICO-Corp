using System.Linq;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Spray : MonoBehaviour, ICleanable
{
    private SprayPlacerHudController sprayController;//holds the spray controller
    private Shooting shotDetector;
    public Destroyable destroyableObject;//the object the spray inherits the destroyable trait from if placed on glass
    public Absorb reflectingDestroyableObject;//the same as above but for armoured glass as it has a different component
    public int savedSpray;//the spray that this object is. used for save syste,
    public bool spawned; //if this is true, the save sytem ignores it. this prevents an exponential amount of sprays being saved - Nova
    private int listCycler = 0;
    private Collider[] destructibles;

                      public bool destructible;//used to save the destructible state of the decal if it was intiially placed on glass
    [HideInInspector] public Vector3 Position;//the stored position of the decal. useed for save system
    [HideInInspector] public Quaternion rotation;//the stored rotation of the decal, used for save system
    //coded by sawyer
    private void OnEnable()
    {
        Invoke(nameof(SaveSprayPosition), 0.02f);//fires the function to save the decal position, on a delay to avoid loss of of information
        sprayController = FindFirstObjectByType<SprayPlacerHudController>();//gets the spray controller
        shotDetector = FindFirstObjectByType<Shooting>();

      
           if (sprayController.firing)
            if (sprayController.hit.transform.GetComponent<Destroyable>() != null)//checks if the original hit object had a destroyable component
            {
                destroyableObject = sprayController.hit.transform.GetComponent<Destroyable>();//if it did, store the component
                destructible = true;//and save the destructible state
                sprayController.firing = false;
            }
            else if (sprayController.hit.transform.GetComponent<Absorb>() != null)//checks if it has the armoured glass component 
            {
                reflectingDestroyableObject = sprayController.hit.transform.GetComponent<Absorb>();//if it does, stores that component
                destructible = true;//and saves the destructible state
                sprayController.firing = false;
            }
        Invoke(nameof(destructionCheck), 0.02f);

    }

    private void destructionCheck()
    {
        if (destructible == true)//checks if the saved decal had the destructible trait
        {
            destructibles = Physics.OverlapSphere(gameObject.transform.position, 1);
            Sorter();
          
        }
    }

    private void Sorter()
    {
        if (destructibles[listCycler].GetComponent<Destroyable>())
        {
            destroyableObject = destructibles[listCycler].GetComponent<Destroyable>();//store the destroyable component if it exists
        }

        if (destructibles[listCycler].GetComponent<Absorb>())
        {
            reflectingDestroyableObject = destructibles[listCycler].GetComponent<Absorb>();//store the absorb component if it exists
        }
        listCycler += 1;

        if (listCycler < destructibles.Length)
        {
            Invoke(nameof(Sorter), 0.01f);
        }
    }
    public GameObject GetGameObject()
    {
        return gameObject;//allows save system to get the spray object
    }

    public void GlassCheck()//used to avoid sprays entering update
    {
        if (destructible)
        {
            if (destroyableObject.shot || destroyableObject == null)//checks if the glass has been destroyed
            {
                Destroy(gameObject.GetComponentInChildren<MeshRenderer>());
            }
        }
     
    }
    public void ArmoredGlassCheck()
    {
        if (destructible)
        {
            if (reflectingDestroyableObject.blasted || reflectingDestroyableObject == null)//checks if the armoured glass has been destroyed
            {
                Destroy(gameObject.GetComponentInChildren<MeshRenderer>());
            }
        }
     
    }

    private void SaveSprayPosition()//used for save system
    {
        Position = gameObject.transform.position;//stores the position of the spray
        rotation = gameObject.transform.rotation;//stores the rotation of the spray
    }
}
