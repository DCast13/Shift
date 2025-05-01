using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BouncerController : MonoBehaviour
{
    [Tooltip("Upward impulse at start")]
    public float launchForce = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // One-time kick so it doesn’t just sit there
        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    }

    // Optional: ensure each bounce is full strength
    void OnCollisionEnter(Collision col)
    {
        Vector3 v = rb.velocity;
        v.y = launchForce;
        rb.velocity = v;
    }
}
