using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    [Header("Bullet Setup")]
    [Tooltip("Drag your Bullet_Pistol_A prefab here")]
    public GameObject bulletPrefab;
    [Tooltip("Child transform at the muzzle tip")]
    public Transform muzzlePoint;
    [Tooltip("Speed at which the bullet travels")]
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

        // Spawn bullet at the muzzle
        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzlePoint.position,
            muzzlePoint.rotation);

        // Propel it forward
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = muzzlePoint.forward * bulletSpeed;
        else
            Debug.LogWarning("PistolShoot: no Rigidbody on bulletPrefab");

        //Debug.Break();
    }
}
