using UnityEngine;

public class MenuLightMovement : MonoBehaviour
{

    //this litteraly JUST moves the lights in the main menu. thats it. dont touch this. it doesnt do anything important. - Nova
    private void Update()
    {
        transform.Translate(transform.right*2f*Time.deltaTime);
        if (transform.position.x > 25f)
        {
            transform.position = new Vector3(-30f, transform.position.y, transform.position.z);
        }
    }
}
