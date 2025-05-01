using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Prefabs")]
    public GameObject weaponWheelPrefab;

    [Header("Systems")]
    public RealitySwitcher realitySwitcher;

    private GameObject weaponWheelInstance;

    private void Awake()
    {
        // Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureEventSystem();
        SetupWeaponWheel();
        SetupRealitySwitcher();
    }

    private void SetupWeaponWheel()
    {
        if (weaponWheelPrefab != null && weaponWheelInstance == null)
        {
            weaponWheelInstance = Instantiate(weaponWheelPrefab);
            DontDestroyOnLoad(weaponWheelInstance);
        }
    }

    private void SetupRealitySwitcher()
    {
        if (realitySwitcher == null)
        {
            Debug.LogWarning("GameManager: RealitySwitcher is not assigned in the Inspector!");
        }
        else
        {
            Debug.Log("GameManager: RealitySwitcher linked successfully.");
        }
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
            DontDestroyOnLoad(eventSystem);
            Debug.Log("GameManager: Created missing EventSystem.");
        }
    }

    // Optional helper function if you want to call these easily elsewhere
    public void SwitchReality()
    {
        if (realitySwitcher != null)
        {
            realitySwitcher.SwitchReality();
        }
    }

    public void ToggleWeaponWheel(bool active)
    {
        if (weaponWheelInstance != null)
        {
            weaponWheelInstance.SetActive(active);
        }
    }
}
