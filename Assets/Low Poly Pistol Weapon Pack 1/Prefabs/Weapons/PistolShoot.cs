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

    [Header("Audio Setup")]
    [Tooltip("Drag your imported gunshot AudioClip here")]
    public AudioClip gunFireSound;
    private AudioSource audioSource;

    void Awake()
    {
        // grab (or add) the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        // sanity check
        if (bulletPrefab == null || muzzlePoint == null || gunFireSound == null)
        {
            Debug.LogWarning("PistolShoot: missing bulletPrefab, muzzlePoint, or gunFireSound reference");
            return;
        }

        // 1) spawn and fire the bullet
        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzlePoint.position,
            muzzlePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = muzzlePoint.forward * bulletSpeed;

        // 2) play the gunshot sound
        audioSource.PlayOneShot(gunFireSound);
    }
}
