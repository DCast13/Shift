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

    public bool selected = false; // make this public

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

    public void Clicked()
    {
        WeaponWheelController.Instance.ButtonClicked(this);
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

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }
}
