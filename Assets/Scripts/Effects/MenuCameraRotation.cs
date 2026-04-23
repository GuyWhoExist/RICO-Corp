using UnityEngine;

public class MenuCameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0 ) * Time.deltaTime);
    }
}
