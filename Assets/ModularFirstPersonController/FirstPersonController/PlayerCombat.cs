using UnityEngine;

// Make sure your enums exist somewhere (WeaponType, AbilityType)

public class PlayerCombat : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject swordPrefab;

    [Header("Ability Scripts")]
    [SerializeField] private WarpAbility warpAbility;
    [SerializeField] private StasisAbility stasisAbility;

    [Header("Weapon Attachment")]
    [SerializeField] private Transform weaponParent;

    private GameObject currentWeapon;
    private WeaponType currentWeaponType = WeaponType.None;
    private AbilityType currentAbilityType = AbilityType.None;

    void Start()
    {
        WeaponWheelController.selectedWeaponType = WeaponType.Pistol;
        WeaponWheelController.selectedAbilityType = AbilityType.Warp;

        if (pistolPrefab != null)
        {
            currentWeapon = weaponParent
                ? Instantiate(pistolPrefab, weaponParent.position, weaponParent.rotation, weaponParent)
                : Instantiate(pistolPrefab, transform.position, transform.rotation);

            if (weaponParent != null)
            {
                currentWeapon.transform.localPosition = Vector3.zero;
                currentWeapon.transform.localRotation = Quaternion.identity;
            }

            currentWeaponType = WeaponType.Pistol;
        }

        if (warpAbility != null) warpAbility.enabled = true;
        if (stasisAbility != null) stasisAbility.enabled = false;

        currentAbilityType = AbilityType.Warp;
    }

    void Update()
    {
        WeaponType selectedWep = WeaponWheelController.selectedWeaponType;
        if (selectedWep != currentWeaponType && selectedWep != WeaponType.None)
        {
            EquipWeapon(selectedWep);
        }

        AbilityType selectedAbility = WeaponWheelController.selectedAbilityType;
        if (selectedAbility != currentAbilityType && selectedAbility != AbilityType.None)
        {
            ActivateAbility(selectedAbility);
        }

        HandleAbilityInput();
    }

    private void EquipWeapon(WeaponType newWeaponType)
    {
        if (currentWeapon != null) Destroy(currentWeapon);

        GameObject newWeaponPrefab = null;

        switch (newWeaponType)
        {
            case WeaponType.Pistol:
                newWeaponPrefab = pistolPrefab;
                break;
            case WeaponType.Sword:
                newWeaponPrefab = swordPrefab;
                break;
        }

        if (newWeaponPrefab != null)
        {
            currentWeapon = weaponParent
                ? Instantiate(newWeaponPrefab, weaponParent.position, weaponParent.rotation, weaponParent)
                : Instantiate(newWeaponPrefab, transform.position, transform.rotation);

            if (weaponParent != null)
            {
                currentWeapon.transform.localPosition = Vector3.zero;
                currentWeapon.transform.localRotation = Quaternion.identity;
            }
        }

        currentWeaponType = newWeaponType;
    }

    private void ActivateAbility(AbilityType newAbilityType)
    {
        if (warpAbility != null) warpAbility.enabled = false;
        if (stasisAbility != null) stasisAbility.enabled = false;

        switch (newAbilityType)
        {
            case AbilityType.Warp:
                if (warpAbility != null) warpAbility.enabled = true;
                break;
            case AbilityType.Stasis:
                if (stasisAbility != null) stasisAbility.enabled = true;
                break;
        }

        currentAbilityType = newAbilityType;
    }

    private void HandleAbilityInput()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            if (currentAbilityType == AbilityType.Warp && warpAbility != null && warpAbility.enabled)
            {
                warpAbility.StartWarpAim();
            }
            else if (currentAbilityType == AbilityType.Stasis && stasisAbility != null && stasisAbility.enabled)
            {
                stasisAbility.ActivateStasis();
            }
        }
    }
}
