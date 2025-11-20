using Unity.VisualScripting;
using UnityEngine;

public class Casting : MonoBehaviour
{
    [SerializeField] private float speed;

    public static implicit operator float(Casting casting)
    {
        return 5f;
    }

    void Start()
    {
        int myInt = 5;
        float myFloat = myInt; //implicitly cast
        //this doesnt work the other way around, but you knew that already, right?

        float f = 5f;
        int i = (int)f; //explicitly cast
        float pi = 3.14159265358979f;
        double pi_d = 3.14159265358979f;

        Debug.Log(pi);
        Debug.Log(pi_d);

        Vector2 myV2 = new Vector2(3, 4);
        Vector3 myV3 = myV2;
        Vector2 otherV2 = myV3;
        Casting myCasting = this;
        float x = myCasting;

    }

    public struct Vector5
    {
        public float a; 
        public float b; 
        public float c; 
        public float d; 
        public float e;
    }


}
