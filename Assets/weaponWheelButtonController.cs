using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class weaponWheelButtonController : MonoBehaviour
{
    public int ID;
    public Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    public bool selected = false;
    public Sprite icon;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
        weaponWheelController.ID = ID;
    }

    public void Deselected()
    {
        selected = false;
        weaponWheelController.ID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("hovered", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("hovered", false);
        itemText.text = "";
    }
}
