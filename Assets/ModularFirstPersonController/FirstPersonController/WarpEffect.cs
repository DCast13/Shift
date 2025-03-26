using UnityEngine;

public class WarpEffect : MonoBehaviour
{
    public GameObject warpEffectPrefab;
    public float effectDuration = 0.5f;

    public void TriggerWarpEffect(Vector3 position)
    {
        GameObject warpEffect = Instantiate(warpEffectPrefab, position, Quaternion.identity);
        Destroy(warpEffect, effectDuration);
    }
}
