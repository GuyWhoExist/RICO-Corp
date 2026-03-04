using Unity.VisualScripting;
using UnityEngine;

public class Spray : MonoBehaviour, ICleanable
{
    private SprayPlacerHudController sprayController;
    private RaycastHit hit;
    private Destroyable destroyableObject;
    private Absorb armouredGlass;
    public float savedSpray;
    [HideInInspector] public Vector3 Position;
    [HideInInspector] public Quaternion rotation;
    //coded by sawyer
    private void OnEnable()
    {

        Position = this.transform.position;
        rotation = this.transform.rotation;
        if (FindFirstObjectByType<SprayPlacerHudController>())
        {
            sprayController = FindFirstObjectByType<SprayPlacerHudController>();
        }
       
        //Debug.Log("checking for spraycontroller...");
        if (sprayController != null)
            //Debug.Log("spraycontroller found!");
        if (sprayController.hit.transform.GetComponent<Destroyable>() != null)
        {
            destroyableObject = sprayController.hit.transform.GetComponent<Destroyable>();
            //Debug.Log("ok, destroy this");
        }
        else if (sprayController.hit.transform.GetComponent<Absorb>() != null) 
        {
            armouredGlass = sprayController.hit.transform.GetComponent<Absorb>();
            //Debug.Log("it is armoured glass, still destroy this");
        }
        else
        {

        }
            //Debug.Log("ok, don't destroy this");
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void Update()
    {
       if (destroyableObject != null)
       {
            if (destroyableObject.shot == true)
            {
                Destroy(gameObject);
            }
       }
       else if (armouredGlass != null)
        {
            if(armouredGlass.blasted == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
