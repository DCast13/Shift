// WarpAbility.cs
using UnityEngine;
using System.Collections;

public class WarpAbility : MonoBehaviour
{
    [Header("Warp Settings")]
    public float maxWarpDistance = 50f;
    public float slowMotionTimeScale = 0.3f;
    public float returnToNormalSpeed = 5f;
    public GameObject warpMarkerPrefab;

    [Header("Cooldown Settings")]
    public float cooldownDuration = 5f;
    private bool isCooldown = false;

    private Camera playerCamera;
    private GameObject warpMarkerInstance;
    private Vector3 targetPosition;
    private bool isAimingWarp = false;

    private void Start()
    {
        playerCamera = Camera.main;

        if (warpMarkerPrefab != null)
        {
            warpMarkerInstance = Instantiate(warpMarkerPrefab);
            warpMarkerInstance.SetActive(false);
        }
        else
        {
            Debug.LogWarning("WarpAbility: Warp Marker Prefab not assigned.");
        }
    }

    private void Update()
    {
        if (isAimingWarp)
        {
            UpdateWarpTarget();

            if (Input.GetMouseButtonUp(1))
            {
                PerformWarp();
            }
        }
    }

    /// <summary>
    /// Called externally (from PlayerCombat) to start aiming for warp.
    /// </summary>
    public void StartWarpAim()
    {
        if (isCooldown)
        {
            Debug.Log("Warp on cooldown.");
            return;
        }
        if (!isAimingWarp)
        {
            isAimingWarp = true;

            // Slow down time
            Time.timeScale = slowMotionTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            if (warpMarkerInstance != null)
                warpMarkerInstance.SetActive(true);
        }
    }

    private void UpdateWarpTarget()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxWarpDistance))
        {
            targetPosition = hit.point;
        }
        else
        {
            // Always project forward max distance
            targetPosition = playerCamera.transform.position + (playerCamera.transform.forward.normalized * maxWarpDistance);
        }

        if (warpMarkerInstance != null)
        {
            warpMarkerInstance.transform.position = targetPosition;
        }
    }

    private void PerformWarp()
    {
        if (warpMarkerInstance != null)
            warpMarkerInstance.SetActive(false);

        isAimingWarp = false;

        // Warp the player
        transform.position = targetPosition;

        // Smoothly return time to normal
        StartCoroutine(SmoothReturnToNormalTime());

        // Begin cooldown
        isCooldown = true;
        StartCoroutine(WarpCooldownRoutine());
    }

    private IEnumerator SmoothReturnToNormalTime()
    {
        float t = 0f;
        float startScale = Time.timeScale;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * returnToNormalSpeed;
            Time.timeScale = Mathf.Lerp(startScale, 1f, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private IEnumerator WarpCooldownRoutine()
    {
        yield return new WaitForSecondsRealtime(cooldownDuration);
        isCooldown = false;
        Debug.Log("Warp ready.");
    }
}
