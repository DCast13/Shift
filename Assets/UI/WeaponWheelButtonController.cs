using UnityEngine;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{
    public WeaponType weaponType = WeaponType.None;
    public AbilityType abilityType = AbilityType.None;
    public bool isWeapon;

    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    private bool selected = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (selected)
        {
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        // Deselect only buttons of the same type
        WeaponWheelButtonController[] allButtons = FindObjectsOfType<WeaponWheelButtonController>();

        foreach (var button in allButtons)
        {
            if (button.isWeapon == this.isWeapon)
            {
                button.Deselected();
            }
        }

        // Now select this button
        selected = true;

        if (isWeapon)
        {
            WeaponWheelController.selectedWeaponType = weaponType;
        }
        else
        {
            WeaponWheelController.selectedAbilityType = abilityType;
        }
    }

    public void Deselected()
    {
        selected = false;
        // No longer reset the global selected weapon/ability here!
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }
}
