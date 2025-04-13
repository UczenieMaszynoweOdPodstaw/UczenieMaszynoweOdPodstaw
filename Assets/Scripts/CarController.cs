using UnityEngine;

public class CarController : MonoBehaviour
{
    public const int ACTION_COUNT = 4;
    [SerializeField] float accelerationForce;
    [SerializeField] float maxSpeed;
    [SerializeField] float turnForce;
    [SerializeField] float maxTurnSpeed;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Accelerate()
    {
        rb.AddForce(transform.forward * accelerationForce, ForceMode.Acceleration);
    }
    public void Brake()
    {
        rb.AddForce(transform.forward * accelerationForce * -0.75f, ForceMode.Acceleration);
    }
    public void TurnLeft()
    {
        rb.AddTorque(transform.up * turnForce * -1);
    }
    public void TurnRight()
    {
        rb.AddTorque(transform.up * turnForce);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * rb.linearVelocity.magnitude;
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
        if (rb.angularVelocity.magnitude > maxTurnSpeed)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * maxTurnSpeed;
        }
    }

}
