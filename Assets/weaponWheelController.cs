using UnityEngine;
using UnityEngine.UI;

public class weaponWheelController : MonoBehaviour
{

    public Animator anim;
    public bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID;
    public static int abilityID;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            weaponWheelSelected = !weaponWheelSelected;
        }

        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }

        switch (weaponID)
        {
            case 0: // No weapon selected
                selectedItem.sprite = noImage;
                break;
            case 1: // Pistol
                Debug.Log("Pistol");
                break;
            case 2: // Sword
                Debug.Log("Sword");
                break;
        }
    }
}
