using UnityEngine;

public class DistortionAnimator : MonoBehaviour
{
    public Material material; // Assign the EtherealParticleMaterial here
    public float speed = 1f; // Speed of the distortion animation

    private void Update()
    {
        if (material != null)
        {
            float timeOffset = material.GetFloat("_TimeOffset");
            timeOffset += Time.deltaTime * speed;
            material.SetFloat("_TimeOffset", timeOffset);
        }
    }
}