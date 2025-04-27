using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponWheelController : MonoBehaviour
{
    public static WeaponWheelController Instance { get; private set; }

    public Animator anim;
    private bool weaponWheelSelected = false;

    public Image selectedWeapon;
    public Image selectedAbility;
    public Sprite noImage;

    public static WeaponType selectedWeaponType = WeaponType.None;
    public static AbilityType selectedAbilityType = AbilityType.None;

    private WeaponType lastWeaponType = WeaponType.None;
    private AbilityType lastAbilityType = AbilityType.None;

    public List<WeaponWheelButtonController> allButtons = new List<WeaponWheelButtonController>();

    void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Find all buttons (or assign manually in inspector if you prefer)
        allButtons.AddRange(FindObjectsOfType<WeaponWheelButtonController>());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            OpenWeaponWheel();
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            CloseWeaponWheel();
        }

        UpdateSelectionUI();
    }

    private void OpenWeaponWheel()
    {
        weaponWheelSelected = true;
        anim.SetBool("OpenWeaponWheel", true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Optionally: Tell your player script to disable camera look here if needed
    }

    private void CloseWeaponWheel()
    {
        weaponWheelSelected = false;
        anim.SetBool("OpenWeaponWheel", false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Optionally: Tell your player script to enable camera look here again
    }

    private void UpdateSelectionUI()
    {
        if (selectedWeaponType != lastWeaponType)
        {
            lastWeaponType = selectedWeaponType;

            switch (selectedWeaponType)
            {
                case WeaponType.Pistol:
                    selectedWeapon.sprite = Resources.Load<Sprite>("Sprites/PistolIcon") ?? noImage;
                    break;
                case WeaponType.Sword:
                    selectedWeapon.sprite = Resources.Load<Sprite>("Sprites/SwordIcon") ?? noImage;
                    break;
                default:
                    selectedWeapon.sprite = noImage;
                    break;
            }
        }

        if (selectedAbilityType != lastAbilityType)
        {
            lastAbilityType = selectedAbilityType;

            switch (selectedAbilityType)
            {
                case AbilityType.Warp:
                    selectedAbility.sprite = Resources.Load<Sprite>("Sprites/WarpIcon") ?? noImage;
                    break;
                case AbilityType.Stasis:
                    selectedAbility.sprite = Resources.Load<Sprite>("Sprites/StasisIcon") ?? noImage;
                    break;
                default:
                    selectedAbility.sprite = noImage;
                    break;
            }
        }
    }


    public void ButtonClicked(WeaponWheelButtonController button)
    {
        // Deselect all buttons of same type
        foreach (var b in allButtons)
        {
            if (b.isWeapon == button.isWeapon)
            {
                b.Deselect();
            }
        }

        // Select clicked button
        button.Select();

        if (button.isWeapon)
        {
            selectedWeaponType = button.weaponType;
        }
        else
        {
            selectedAbilityType = button.abilityType;
        }
    }
}
