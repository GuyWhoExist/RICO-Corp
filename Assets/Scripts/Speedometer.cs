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
    private Color Purple = new Color32(115, 15, 240, 255);
    private void Update()
    {
        //speed = playerRigidBody.linearVelocity;
        //rotationalSpeed = playerRigidBody.angularVelocity;
        //averageLinearSpeed = speed.x + speed.z;
        //averageRotationalSpeed = rotationalSpeed.x + rotationalSpeed.z;
        //averageSpeed = (averageLinearSpeed + averageRotationalSpeed) * 4;
        averageLinearSpeed = playerRigidBody.linearVelocity.magnitude;
        averageRotationalSpeed = playerRigidBody.angularVelocity.magnitude;
        averageSpeed = averageRotationalSpeed + averageLinearSpeed * 4;
        if (averageSpeed > 0)
        {
            averageSpeed *= -1;
        }
        if (averageSpeed * -1 > 170)
        {
            averageSpeed = -170;
        }
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, averageSpeed);

           
        if (averageSpeed * -1 > 30)
        {
            speedometerBacker.color = Color.yellow;
            if (averageSpeed * -1 > 60)
            {
                speedometerBacker.color = orange;
                if (averageSpeed * -1 > 90)
                {
                    speedometerBacker.color = Color.red;
                    if (averageSpeed * -1 > 120)
                    {
                        speedometerBacker.color = Purple;
                    }
                }
            }
        }
        else
            speedometerBacker.color = Color.white;
    }
}
