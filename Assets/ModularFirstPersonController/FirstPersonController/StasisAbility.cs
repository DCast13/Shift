using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StasisAbility : MonoBehaviour
{
    [Header("Stasis Settings")]
    public float stasisDuration = 5f;
    public float worldSlowTimeScale = 0.2f;
    public float cooldownDuration = 5f;

    [Header("UI Elements")]
    public Image cooldownImage; // Assign a UI Image for cooldown fill

    private bool isStasisActive = false;
    private bool isCooldown = false;
    private float originalFixedDeltaTime;

    private void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 1f; // Fully ready at start
        }
    }

    private void Update()
    {
        if (isCooldown && cooldownImage != null)
        {
            cooldownImage.fillAmount += (1f / cooldownDuration) * Time.unscaledDeltaTime;
        }
    }

    public void ActivateStasis()
    {
        if (!isStasisActive && !isCooldown)
        {
            StartCoroutine(StasisRoutine());
        }
    }

    private IEnumerator StasisRoutine()
    {
        isStasisActive = true;

        Debug.Log("Stasis Activated!");

        Time.timeScale = worldSlowTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * worldSlowTimeScale;

        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f; // Start cooldown

        isCooldown = true;

        yield return new WaitForSecondsRealtime(stasisDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        isStasisActive = false;

        // Start cooldown separately
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        float cooldownTimer = cooldownDuration;

        while (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.unscaledDeltaTime;
            yield return null;
        }

        isCooldown = false;
        if (cooldownImage != null)
            cooldownImage.fillAmount = 1f; // Cooldown ready
    }
}
