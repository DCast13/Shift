using UnityEngine;
using System.Collections;

public class WarpAbility : MonoBehaviour
{
    [Header("Warp Settings")]
    public float maxWarpDistance = 50f;
    public LayerMask teleportableSurfaces;
    public GameObject warpMarkerPrefab;
    public GameObject warpEffectPrefab;

    [Header("Slow Motion Settings")]
    public float slowMoTimeScale = 0.3f;
    public float returnToNormalSpeed = 5f;

    private Camera playerCamera;
    private GameObject warpMarker;
    private Vector3 targetPosition;
    private bool isAimingWarp = false;

    void Start()
    {
        playerCamera = Camera.main;

        if (warpMarkerPrefab)
        {
            warpMarker = Instantiate(warpMarkerPrefab);
            warpMarker.SetActive(false);
        }
    }

    void Update()
    {
        if (isAimingWarp)
        {
            UpdateWarpMarker();

            if (Input.GetMouseButtonUp(1)) // Release right-click
            {
                PerformWarp();
                isAimingWarp = false;
            }
        }
    }

    public void StartWarpAim()
    {
        if (!isAimingWarp)
        {
            isAimingWarp = true;
            Time.timeScale = slowMoTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    void UpdateWarpMarker()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxWarpDistance, teleportableSurfaces))
        {
            targetPosition = hit.point + Vector3.up * 0.1f;
        }
        else
        {
            targetPosition = playerCamera.transform.position + playerCamera.transform.forward * maxWarpDistance;
        }

        warpMarker.SetActive(true);
        warpMarker.transform.position = targetPosition;
    }

    void PerformWarp()
    {
        if (!warpMarker.activeSelf) return;

        if (warpEffectPrefab)
        {
            Instantiate(warpEffectPrefab, transform.position, Quaternion.identity);
        }

        transform.position = targetPosition;

        if (warpEffectPrefab)
        {
            Instantiate(warpEffectPrefab, transform.position, Quaternion.identity);
        }

        warpMarker.SetActive(false);
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
        Time.fixedDeltaTime = 0.02f;
    }
}
