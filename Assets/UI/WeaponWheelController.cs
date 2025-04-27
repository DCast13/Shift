using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;

    public Image selectedWeapon;
    public Image selectedAbility;
    public Sprite noImage;

    public static WeaponType selectedWeaponType = WeaponType.None;
    public static AbilityType selectedAbilityType = AbilityType.None;

    private WeaponType lastWeaponType = WeaponType.None;
    private AbilityType lastAbilityType = AbilityType.None;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            weaponWheelSelected = !weaponWheelSelected;
        }

        anim.SetBool("OpenWeaponWheel", weaponWheelSelected);

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
}
