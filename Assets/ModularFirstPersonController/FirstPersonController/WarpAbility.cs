using UnityEngine;
using System.Collections;

public class WarpAbility : MonoBehaviour
{
    [Header("Warp Settings")]
    public float maxWarpDistance = 50f;  // Maximum warp range
    public LayerMask teleportableSurfaces; // Define surfaces you can warp to
    public GameObject warpMarkerPrefab; // Visual marker for warp location
    public GameObject warpEffectPrefab; // Distortion effect when warping

    [Header("Slow Motion Settings")]
    public float slowMoTimeScale = 0.3f; // Time scale while aiming (lower = slower)
    public float returnToNormalSpeed = 5f; // Speed of time returning to normal

    private Camera playerCamera;
    private GameObject warpMarker;
    private Vector3 targetPosition;
    private bool isAimingWarp = false;

    void Start()
    {
        playerCamera = Camera.main;

        // Instantiate the warp marker but disable it initially
        if (warpMarkerPrefab)
        {
            warpMarker = Instantiate(warpMarkerPrefab);
            warpMarker.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Start aiming
        {
            isAimingWarp = true;
            Time.timeScale = slowMoTimeScale; // Enable slow motion
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust physics step for slow motion
        }

        if (Input.GetMouseButton(1)) // While holding RMB, update the warp marker
        {
            UpdateWarpMarker();
        }

        if (Input.GetMouseButtonUp(1) && isAimingWarp) // Release to warp
        {
            PerformWarp();
            isAimingWarp = false;
        }
    }

    void UpdateWarpMarker()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxWarpDistance, teleportableSurfaces))
        {
            // If a surface is hit, warp to it
            targetPosition = hit.point + Vector3.up * 0.1f; // Slight offset to avoid clipping
        }
        else
        {
            // If no surface is found, warp to max range in air
            targetPosition = playerCamera.transform.position + playerCamera.transform.forward * maxWarpDistance;
        }

        // Update the marker position and show it
        warpMarker.SetActive(true);
        warpMarker.transform.position = targetPosition;
    }

    void PerformWarp()
    {
        if (!warpMarker.activeSelf) return;

        // Spawn the warp effect at the current position
        if (warpEffectPrefab)
        {
            Instantiate(warpEffectPrefab, transform.position, Quaternion.identity);
        }

        // Warp the player
        transform.position = targetPosition;

        // Spawn the warp effect at the new position
        if (warpEffectPrefab)
        {
            Instantiate(warpEffectPrefab, transform.position, Quaternion.identity);
        }

        // Hide the marker after warping
        warpMarker.SetActive(false);

        // Gradually return to normal time
        StartCoroutine(ReturnToNormalTime());
    }

    IEnumerator ReturnToNormalTime()
    {
        float t = 0f;
        float startTimeScale = Time.timeScale;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * returnToNormalSpeed;
            Time.timeScale = Mathf.Lerp(startTimeScale, 1f, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // Reset to default physics step
    }
}
