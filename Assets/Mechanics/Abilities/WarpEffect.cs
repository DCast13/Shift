using UnityEngine;

public class WarpEffect : MonoBehaviour
{
    public GameObject warpEffectPrefab;
    public float effectDuration = 0.5f;
    public bool attachToCamera = true; // Option to attach effect to the player's camera

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    public void TriggerWarpEffect(Vector3 position)
    {
        if (warpEffectPrefab == null) return;

        GameObject warpEffect = Instantiate(warpEffectPrefab, position, Quaternion.identity);

        // If in first-person, attach the effect to the camera for visibility
        if (attachToCamera && playerCamera != null)
        {
            warpEffect.transform.SetParent(playerCamera.transform, false);
            warpEffect.transform.localPosition = Vector3.zero; // Keep it centered on screen
        }

        // Auto-destroy after the set duration
        Destroy(warpEffect, effectDuration);
    }
}
