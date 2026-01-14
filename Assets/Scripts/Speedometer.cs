using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField] Rigidbody playerRigidBody;
    [SerializeField] RawImage speedometerBacker;
    private Vector3 speed;
    private float averageLinearSpeed;
    private Vector3 rotationalSpeed;
    private float averageRotationalSpeed;
    private float averageSpeed;
    private Color orange = new Color(1.0f, 0.64f, 0.0f);

    private void Awake()
    {
        
    }
    private void Update()
    {
        speed = playerRigidBody.linearVelocity;
        rotationalSpeed = playerRigidBody.angularVelocity;
        averageLinearSpeed = speed.x + 0 + speed.z;
        averageRotationalSpeed = rotationalSpeed.x + 0 + rotationalSpeed.y;
        averageSpeed = (averageLinearSpeed + averageRotationalSpeed) * 4;
        if (averageSpeed > 0)
        {
            averageSpeed *= -1;
        }
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, averageSpeed);
        if (averageSpeed * -1 > 30)
        {
            speedometerBacker.color = Color.yellow;
            if (averageSpeed * -1 > 60)
            {
                speedometerBacker.color = orange;
                if (averageSpeed * -1 > 90 )
                    speedometerBacker.color = Color.red;
            }
        }
        else
            speedometerBacker.color = Color.white;
    }
}
