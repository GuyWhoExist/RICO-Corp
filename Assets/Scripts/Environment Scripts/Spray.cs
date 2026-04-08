using UnityEngine;

public class Spray : MonoBehaviour, ICleanable
{
    private SprayPlacerHudController sprayController;//holds the spray controller
    private RaycastHit hit;//stores the object hit by a raycast
    private Destroyable destroyableObject;//the object the spray inherits the destroyable trait from if placed on glass
    private Absorb armoredGlass;///the same as above but for armoured glass as it has a different component
    public float savedSpray;//the spray that this object is. used for save syste,

    [HideInInspector]public bool destructible;//used to save the destructible state of the decal if it was intiially placed on glass
    [HideInInspector] public Vector3 Position;//the stored position of the decal. useed for save system
    [HideInInspector] public Quaternion rotation;//the stored rotation of the decal, used for save system
    //coded by sawyer
    private void OnEnable()
    {
        Invoke(nameof(SaveSprayPosition), 0.02f);//fires the function to save the decal position, on a delay to avoid loss of of information
        sprayController = FindFirstObjectByType<SprayPlacerHudController>();//gets the spray controller

        if (destructible)//checks if the saved decal had the destructible trait
        {
            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward * -1, out hit, 1f))//if it did, fire a raycast backward to get the object it was laced ons destructible component
            {
                if (hit.transform.GetComponent<Destroyable>())//checks if the hit object contains the destroyable component
                {
                    destroyableObject = hit.transform.GetComponent<Destroyable>();//store the destroyable component if it exists
                    InvokeRepeating(nameof(GlassCheck), .1f, .1f);//fires the glass checker function
                }
                else if (hit.transform.GetComponent<Absorb>())//also checks if the hit object has the absorb component
                {
                    armoredGlass = hit.transform.GetComponent<Absorb>();//store the absorb component if it exists
                    InvokeRepeating(nameof(ArmoredGlassCheck), .1f, .1f);//fires the armored glass checker function
                }
               
            }
            else if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, 1f))//if nothing is found in the back, check the front just to be sure
            {
                if (hit.transform.GetComponent<Destroyable>())//checks if the hit object contains the destroyable component
                {
                    destroyableObject = hit.transform.GetComponent<Destroyable>();//store the destroyable component if it exists
                    InvokeRepeating(nameof(GlassCheck), .1f, .1f);//fires the glass checker function
                }
                else if (hit.transform.GetComponent<Absorb>())//also checks if the hit object has the absorb component
                {
                    armoredGlass = hit.transform.GetComponent<Absorb>();//store the absorb component if it exists
                    InvokeRepeating(nameof(ArmoredGlassCheck), .1f, .1f);//fires the armored glass checker function
                }
            }
            else//if both are false
            {
                destructible = false;//code may have been a false positive, disregard
            }
        }

        if (sprayController.hit.transform.GetComponent<Destroyable>() != null)//checks if the original hit object had a destroyable component
        {
            destroyableObject = sprayController.hit.transform.GetComponent<Destroyable>();//if it did, store the component
            InvokeRepeating(nameof(GlassCheck), .1f, .1f);//fires the glass checker function
            destructible = true;//and save the destructible state
        }
        else if (sprayController.hit.transform.GetComponent<Absorb>() != null)//checks if it has the armoured glass component 
        {
            armoredGlass = sprayController.hit.transform.GetComponent<Absorb>();//if it does, stores that component
            InvokeRepeating(nameof(GlassCheck), .1f, .1f);//fires the glass checker function
            destructible = true;//and saves the destructible state
        }
        
     
    }
    public GameObject GetGameObject()
    {
        return gameObject;//allows save system to get the spray object
    }

    private void GlassCheck()//used to avoid sprays entering update
    {
            if (destroyableObject.shot == true || destroyableObject == null)//checks if the glass has been destroyed
            {
                Destroy(gameObject);//if it has destroy it
            }
    }
    private void ArmoredGlassCheck()
    {  
       if (armoredGlass.blasted == true || armoredGlass == null)//checks if the armoured glass has been destroyed
       {
                Destroy(gameObject);//if so destroys this
       }
    }

    private void SaveSprayPosition()//used for save system
    {
        Position = gameObject.transform.position;//stores the position of the spray
        rotation = gameObject.transform.rotation;//stores the rotation of the spray
    }
}
