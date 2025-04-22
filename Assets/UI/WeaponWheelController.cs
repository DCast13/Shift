using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            weaponWheelSelected = !weaponWheelSelected;
        }

        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
            selectedItem.sprite = noImage;
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
            selectedItem.sprite = noImage;
        }

        switch (weaponID)
        {
            case 0: // No weapon selected
                selectedItem.sprite = noImage;
                break;
            case 1: // Pistol
                Debug.Log("Pistol selected");
                selectedItem.sprite = Resources.Load<Sprite>("PistolIcon");
                break;
            case 2: // Sword
                Debug.Log("Sword selected");
                selectedItem.sprite = Resources.Load<Sprite>("SwordIcon");
                break;
            case 3: // Warp
                Debug.Log("Warp selected");
                selectedItem.sprite = Resources.Load<Sprite>("WarpIcon");
                break;
            case 4: // Stasis
                Debug.Log("Stasis selected");
                selectedItem.sprite = Resources.Load<Sprite>("StasisIcon");
                break;

        }
    }
}
