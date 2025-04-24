using UnityEngine;

public class RealitySwitcher : MonoBehaviour
{
    public GameObject reality1;
    public GameObject reality2;
    public Material Fantasy_Skybox;
    public Material SciFi_Skybox;
    public Transform player; // Reference to the player object
    public LayerMask reality1Layer; // Set in Inspector to Reality1 Layer
    public LayerMask reality2Layer; // Set in Inspector to Reality2 Layer
    private bool isReality1Active = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab Pressed!");
            AttemptSwitchReality();
        }
    }

    void AttemptSwitchReality()
    {
        if (CanSwitch())
        {
            SwitchReality();
        }
        else
        {
            Debug.Log("Blocked! You're inside an object from the other reality.");
        }
    }

    bool CanSwitch()
    {
        LayerMask checkLayer = isReality1Active ? reality2Layer : reality1Layer;

        // Check if player is colliding with objects from the opposite reality
        Collider[] colliders = Physics.OverlapSphere(player.position, 0.5f, checkLayer);

        return colliders.Length == 0; // Can switch if no colliders detected
    }

    void SwitchReality()
    {
        isReality1Active = !isReality1Active; // Toggle state

        reality1.SetActive(isReality1Active);
        reality2.SetActive(!isReality1Active); // Corrected

        // Swap skybox
        RenderSettings.skybox = isReality1Active ? Fantasy_Skybox : SciFi_Skybox;
        DynamicGI.UpdateEnvironment(); // Refresh lighting

        Debug.Log("Reality switched to: " + (isReality1Active ? "Reality 1" : "Reality 2"));
    }
}
