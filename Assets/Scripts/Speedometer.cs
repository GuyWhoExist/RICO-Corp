using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    //Controls the fuel meter on the speedometer - Nova
    //Coded by Sawyer

    [SerializeField] Rigidbody playerRigidBody;
    [SerializeField] RawImage speedometerBacker;
    [SerializeField] Image speedBoostTracker1;
    [SerializeField] Image speedBoostTracker2;
    [SerializeField] Image speedBoostTracker3;
    [SerializeField] Image speedBoostTracker4;
    [SerializeField] Image speedBoostTracker5;
    [SerializeField] Image speedBoostTracker6;
    private SpeedBoost speedBoostValue;
    private Vector3 speed;
    private float averageLinearSpeed;
    private Vector3 rotationalSpeed;
    private float averageRotationalSpeed;
    private float averageSpeed;
    private Color orange = new Color(1.0f, 0.64f, 0.0f);
    private Color Purple = new Color32(115, 15, 240, 255);

    private void Awake()
    {

        if (FindFirstObjectByType<SpeedBoost>())
        {
            speedBoostValue = FindFirstObjectByType<SpeedBoost>();
        }
        else
        {
            Debug.Log("oh no.");
        }
            
    }

    //This method was coded by Nova
    private void UpdateSpeedometer(Image segment, int index, float comparison) //Initial call will be (speedBoostTracker1, 1, 0.4f) - Nova
    {
        if (speedBoostValue.fuel > comparison)
        {
            segment.color = Purple;
        }
        else
        {
            segment.color = Color.white;
        }

        if (index != 6) //we call the method within itself in order to update every speedBoostTracker - Nova
        {
            switch (index)
            {
                case 1:
                    UpdateSpeedometer(speedBoostTracker2, 2, 0.9f);
                    break;
                case 2:
                    UpdateSpeedometer(speedBoostTracker3, 3, 1.4f);
                    break;
                case 3:
                    UpdateSpeedometer(speedBoostTracker4, 4, 1.9f);
                    break;
                case 4:
                    UpdateSpeedometer(speedBoostTracker5, 5, 2.4f);
                    break;
                case 5:
                    UpdateSpeedometer(speedBoostTracker6, 6, 2.9f);
                    break;
            }
        }
    }

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


        /*
         This nested if statement was replaced by the method bellow it. - Nova
         if (speedBoostValue.fuel > 0.4)
        {
            speedBoostTracker1.color = Purple;
            if(speedBoostValue.fuel > 0.9)
            {
                speedBoostTracker2.color = Purple;
                if(speedBoostValue.fuel > 1.4)
                {
                    speedBoostTracker3.color = Purple;
                    if (speedBoostValue.fuel > 1.9)
                    {
                        speedBoostTracker4.color = Purple;
                        if (speedBoostValue.fuel > 2.4)
                        {
                            speedBoostTracker5.color = Purple;
                            if (speedBoostValue.fuel > 2.9)
                            {
                                speedBoostTracker6.color = Purple;
                            }
                            else 
                            {
                                
                            }
                        }
                    }
                }
            }
        }*/
        UpdateSpeedometer(speedBoostTracker1, 1, 0.4f);
    }
    
}
