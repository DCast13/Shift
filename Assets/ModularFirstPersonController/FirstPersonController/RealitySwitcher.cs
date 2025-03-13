using UnityEngine;

public class RealitySwitcher : MonoBehaviour
{
    public GameObject reality1;
    public GameObject reality2;
    public Material skybox1;
    public Material skybox2;
    private bool isReality1Active = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab Pressed!");
            SwitchReality();
        }
    }

    void SwitchReality()
    {
        isReality1Active = !isReality1Active; // Toggle state

        reality1.SetActive(isReality1Active);
        reality2.SetActive(!isReality1Active); // Corrected

        // Swap skybox
        RenderSettings.skybox = isReality1Active ? skybox1 : skybox2;
        DynamicGI.UpdateEnvironment(); // Refresh lighting

        Debug.Log("Reality switched to: " + (isReality1Active ? "Reality 1" : "Reality 2"));
    }
}
