using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    [Header("Bullet Setup")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float bulletSpeed = 25f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        if (bulletPrefab == null || muzzlePoint == null)
        {
            Debug.LogWarning("PistolShoot: Missing prefab or muzzlePoint");
            return;
        }

        // spawn and propel
        GameObject bullet = Instantiate(bulletPrefab,
                                        muzzlePoint.position,
                                        muzzlePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = muzzlePoint.forward * bulletSpeed;
        else
            Debug.LogWarning("PistolShoot: no Rigidbody on bulletPrefab");
    }
}
