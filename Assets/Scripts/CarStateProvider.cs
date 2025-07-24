using UnityEngine;

public class CarStateProvider : MonoBehaviour
{
    [SerializeField] Transform leftSensor;
    [SerializeField] Transform leftFrontSensor;
    [SerializeField] Transform frontSensor;
    [SerializeField] Transform rightFrontSensor;
    [SerializeField] Transform rightSensor;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public CarState GetCarState()
    {
        return new CarState
        {
            Speed = rb.linearVelocity.magnitude,
            LeftDistance = GetSensorDistance(leftSensor),
            LeftFrontDistance = GetSensorDistance(leftFrontSensor),
            FrontDistance = GetSensorDistance(frontSensor),
            RightFrontDistance = GetSensorDistance(rightFrontSensor),
            RightDistance = GetSensorDistance(rightSensor)
        };
    }

    float GetSensorDistance(Transform sensor)
    {
        float maxDistance = 1000;
        float distance = maxDistance;
        RaycastHit raycastHit;
        int trackLayerMask = LayerMask.GetMask("Track");

        if (Physics.Raycast(sensor.position, sensor.forward, out raycastHit, maxDistance, trackLayerMask))
        {
            distance = raycastHit.distance;
        }
        return distance;
    }
}
