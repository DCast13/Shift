using UnityEngine;

// Make sure your enums (WeaponType, AbilityType) are defined elsewhere
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

    // ---- NEW animation fields ----
    private Animator rootAnimator;
    private bool isAttacking = false;
    // --------------------------------

    void Start()
    {
        // ---- NEW grab the Animator on this GameObject ----
        rootAnimator = GetComponent<Animator>();
        // -------------------------------------------------

        // QuickWheel defaults
        WeaponWheelController.selectedWeaponType = WeaponType.Pistol;
        WeaponWheelController.selectedAbilityType = AbilityType.Warp;

        // Spawn default pistol
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

        // Enable warp, disable stasis by default
        if (warpAbility != null) warpAbility.enabled = true;
        if (stasisAbility != null) stasisAbility.enabled = false;

        currentAbilityType = AbilityType.Warp;
    }

    void Update()
    {
        // ---- NEW feed the Animator which weapon is active ----
        if (rootAnimator != null)
            rootAnimator.SetInteger("WeaponType", (int)currentWeaponType);
        // -----------------------------------------------------

        // 1) Weapon Wheel equip (UNCHANGED)
        WeaponType selectedWep = WeaponWheelController.selectedWeaponType;
        if (selectedWep != currentWeaponType && selectedWep != WeaponType.None)
            EquipWeapon(selectedWep);

        // 2) Ability switch (UNCHANGED)
        AbilityType selectedAb = WeaponWheelController.selectedAbilityType;
        if (selectedAb != currentAbilityType && selectedAb != AbilityType.None)
            ActivateAbility(selectedAb);

        // 3) Ability input (RIGHT-CLICK), only if not attacking
        if (!isAttacking)
            HandleAbilityInput();

        // ---- NEW Attack input (LEFT-CLICK), only if not already in an attack ----
        if (!isAttacking && Input.GetMouseButtonDown(0))
            TriggerAttack();
        // -------------------------------------------------------------------------
    }

    private void EquipWeapon(WeaponType newWeaponType)
    {
        // UNCHANGED
        if (currentWeapon != null) Destroy(currentWeapon);

        GameObject newPrefab = null;
        switch (newWeaponType)
        {
            case WeaponType.Pistol: newPrefab = pistolPrefab; break;
            case WeaponType.Sword: newPrefab = swordPrefab; break;
        }

        if (newPrefab != null)
        {
            currentWeapon = weaponParent
                ? Instantiate(newPrefab, weaponParent.position, weaponParent.rotation, weaponParent)
                : Instantiate(newPrefab, transform.position, transform.rotation);

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
        // UNCHANGED
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
        if (Input.GetMouseButtonDown(1)) // right-click
        {
            if (currentAbilityType == AbilityType.Warp &&
                warpAbility != null && warpAbility.enabled)
            {
                warpAbility.StartWarpAim();
            }
            else if (currentAbilityType == AbilityType.Stasis &&
                     stasisAbility != null && stasisAbility.enabled)
            {
                stasisAbility.ActivateStasis();
            }
        }
    }

    // ---- NEW trigger the Animator's Attack transition ----
    private void TriggerAttack()
    {
        if (rootAnimator == null) return;
        isAttacking = true;
        rootAnimator.SetTrigger("Attack");
    }
    // -------------------------------------------------------

    // ---- NEW called by Animation Event at the end of swordSwing & pistolFire ----
    public void OnAttackComplete()
    {
        isAttacking = false;
    }
    // -----------------------------------------------------------------------------
}
