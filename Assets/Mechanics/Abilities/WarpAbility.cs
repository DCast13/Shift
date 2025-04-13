using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarpAbility : MonoBehaviour
{
    [Header("Warp Settings")]
    public float maxWarpDistance = 20f;
    public GameObject warpMarkerPrefab;
    public float groundCheckDistance = 2f;

    [Header("Slow Motion Settings")]
    public float slowMoTimeScale = 0.3f;
    public float returnToNormalSpeed = 5f;

    [Header("Cooldown Settings")]
    public float cooldownDuration = 2f;
    private Image cooldownFiller;
    private RectTransform cooldownFillerRect;
    private float cooldownBarWidth;
    private float lastUsedTime;
    private bool canUseAbility = true;
    private Coroutine returnToNormalTimeCoroutine;

    private Camera playerCamera;
    private GameObject warpMarker;
    private LineRenderer lineRenderer;
    private GameObject groundIndicator;
    private Vector3 targetPosition;
    private bool isAimingWarp = false;
    private Collider playerCollider;
    private float lockedDistance;

    void Start()
    {
        playerCamera = Camera.main;

        playerCollider = GetComponentInParent<Collider>();
        if (playerCollider == null)
        {
            Debug.LogWarning("No collider found on player for WarpAbility!");
        }

        if (warpMarkerPrefab)
        {
            warpMarker = Instantiate(warpMarkerPrefab);
            warpMarker.SetActive(false);

            lineRenderer = warpMarker.GetComponent<LineRenderer>();
            groundIndicator = warpMarker.transform.Find("GroundIndicator")?.gameObject;

            if (lineRenderer == null)
            {
                Debug.LogWarning("WarpMarker prefab is missing a LineRenderer component!");
            }
            if (groundIndicator == null)
            {
                Debug.LogWarning("WarpMarker prefab is missing a GroundIndicator child!");
            }
        }

        lastUsedTime = -cooldownDuration;

        // Find the CooldownFiller within this GameObject's hierarchy
        Transform cooldownFillerTransform = transform.Find("CooldownCanvas/CooldownBar/CooldownFiller");
        if (cooldownFillerTransform != null)
        {
            cooldownFiller = cooldownFillerTransform.GetComponent<Image>();
            if (cooldownFiller != null)
            {
                cooldownFillerRect = cooldownFiller.GetComponent<RectTransform>();
                RectTransform cooldownBarRect = cooldownFillerRect.parent.GetComponent<RectTransform>();
                cooldownBarWidth = cooldownBarRect.sizeDelta.x;
                cooldownFillerRect.sizeDelta = new Vector2(cooldownBarWidth, cooldownFillerRect.sizeDelta.y);
                Debug.Log("CooldownBar width: " + cooldownBarWidth); // Debug to verify the width
            }
            else
            {
                Debug.LogWarning("CooldownFiller found, but it has no Image component!");
            }
        }
        else
        {
            Debug.LogWarning("Could not find CooldownFiller in the hierarchy of " + gameObject.name + "!");
        }

        // Ensure Time.timeScale starts at 1
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    void Update()
    {
        // Update the cooldown status
        if (Time.time >= lastUsedTime + cooldownDuration)
        {
            canUseAbility = true;
        }

        // Update the cooldown filler
        if (cooldownFiller != null)
        {
            float cooldownProgress = Mathf.Clamp01((Time.time - lastUsedTime) / cooldownDuration);
            // Shrink the filler as the cooldown starts, then fill it back up
            float fillerWidth = cooldownBarWidth * -(1f - cooldownProgress); // Shrink from full to 0
            cooldownFillerRect.sizeDelta = new Vector2(fillerWidth, cooldownFillerRect.sizeDelta.y);
        }

        // If the ability is on cooldown, ensure Time.timeScale is 1 and stop any slow-motion
        if (!canUseAbility)
        {
            if (returnToNormalTimeCoroutine != null)
            {
                StopCoroutine(returnToNormalTimeCoroutine);
                returnToNormalTimeCoroutine = null;
            }
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            isAimingWarp = false; // Ensure aiming is disabled during cooldown
            if (warpMarker != null)
            {
                warpMarker.SetActive(false); // Hide the warp marker during cooldown
            }
        }

        // Only allow warp-related actions if the ability is ready
        if (canUseAbility)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isAimingWarp = true;
                Time.timeScale = slowMoTimeScale;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                CalculateWarpDistance();
            }

            if (Input.GetMouseButton(1) && isAimingWarp)
            {
                UpdateWarpMarker();
            }

            if (Input.GetMouseButtonUp(1) && isAimingWarp)
            {
                PerformWarp();
                isAimingWarp = false;
            }
        }
    }

    void CalculateWarpDistance()
    {
        Vector3 rayOrigin = playerCamera.transform.position + playerCamera.transform.forward * 0.2f;
        Ray ray = new Ray(rayOrigin, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxWarpDistance))
        {
            if (playerCollider != null && hit.collider == playerCollider)
            {
                lockedDistance = maxWarpDistance - 0.2f;
            }
            else if (hit.collider.gameObject.activeInHierarchy)
            {
                lockedDistance = Vector3.Distance(rayOrigin, hit.point);
            }
            else
            {
                lockedDistance = maxWarpDistance - 0.2f;
            }
        }
        else
        {
            lockedDistance = maxWarpDistance - 0.2f;
        }

        Vector3 tempPosition = rayOrigin + playerCamera.transform.forward * lockedDistance;
        RaycastHit groundHit;
        if (Physics.Raycast(tempPosition, Vector3.down, out groundHit, groundCheckDistance))
        {
            if ((playerCollider == null || groundHit.collider != playerCollider) && groundHit.collider.gameObject.activeInHierarchy)
            {
                lockedDistance = Vector3.Distance(rayOrigin, groundHit.point);
            }
        }
    }

    void UpdateWarpMarker()
    {
        // Extra safeguard: don't update the marker if the ability is on cooldown
        if (!canUseAbility)
        {
            if (warpMarker != null)
            {
                warpMarker.SetActive(false);
            }
            return;
        }

        Vector3 rayOrigin = playerCamera.transform.position + playerCamera.transform.forward * 0.2f;
        targetPosition = rayOrigin + playerCamera.transform.forward * lockedDistance;

        Ray ray = new Ray(rayOrigin, playerCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, lockedDistance))
        {
            if (playerCollider == null || hit.collider != playerCollider)
            {
                if (hit.collider.gameObject.activeInHierarchy)
                {
                    targetPosition = hit.point + Vector3.up * 0.1f;
                }
            }
        }

        RaycastHit groundHit;
        if (Physics.Raycast(targetPosition, Vector3.down, out groundHit, groundCheckDistance))
        {
            if ((playerCollider == null || groundHit.collider != playerCollider) && groundHit.collider.gameObject.activeInHierarchy)
            {
                targetPosition = groundHit.point + Vector3.up * 0.1f;
            }
        }

        warpMarker.SetActive(true);
        warpMarker.transform.position = targetPosition;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, targetPosition);
            RaycastHit downHit;
            if (Physics.Raycast(targetPosition, Vector3.down, out downHit, 100f))
            {
                lineRenderer.SetPosition(1, downHit.point);
                if (groundIndicator != null)
                {
                    groundIndicator.SetActive(true);
                    groundIndicator.transform.position = downHit.point + Vector3.up * 0.01f;
                    groundIndicator.transform.rotation = Quaternion.LookRotation(Vector3.up);
                }
            }
            else
            {
                lineRenderer.SetPosition(1, targetPosition + Vector3.down * 100f);
                if (groundIndicator != null)
                {
                    groundIndicator.SetActive(false);
                }
            }
        }
    }

    void PerformWarp()
    {
        if (!warpMarker.activeSelf) return;

        transform.position = targetPosition;

        warpMarker.SetActive(false);

        lastUsedTime = Time.time;
        canUseAbility = false;

        // Start the coroutine to return to normal time
        if (returnToNormalTimeCoroutine != null)
        {
            StopCoroutine(returnToNormalTimeCoroutine);
        }
        returnToNormalTimeCoroutine = StartCoroutine(ReturnToNormalTime());
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
        Time.fixedDeltaTime = 0.02f;
        returnToNormalTimeCoroutine = null;
    }
}