using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    [Header("Swish SFX")]
    [Tooltip("Drag your sword swish AudioClip here")]
    public AudioClip swingSound;
    [Tooltip("Volume of the swish (0–1)")]
    [Range(0f, 1f)]
    public float swingVolume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        // grab or add an AudioSource on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // only runs when this sword GameObject is active (i.e. equipped)
        if (Input.GetMouseButtonDown(0))
            PlaySwingSound();
    }

    private void PlaySwingSound()
    {
        if (swingSound != null)
            audioSource.PlayOneShot(swingSound, swingVolume);
    }
}
