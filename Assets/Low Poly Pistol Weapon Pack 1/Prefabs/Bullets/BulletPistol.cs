using UnityEngine;

public class BulletPistol : MonoBehaviour
{
    // Optional: destroy after 5s if it never hits anything
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // You could grab collision.gameObject to apply damage here
        Destroy(gameObject);
    }
}
