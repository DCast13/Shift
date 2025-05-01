using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BouncingCubeController : MonoBehaviour
{
    [Tooltip("Initial upward impulse force")]
    public float initialBounceForce = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Give it a kick off the ground
        rb.AddForce(Vector3.up * initialBounceForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Whenever it hits something, re-bump it up
        // Keeps the bounce consistent instead of losing energy
        Vector3 vel = rb.velocity;
        vel.y = initialBounceForce;
        rb.velocity = vel;
    }
}
