using UnityEngine;
using UnityEngine.UI;

public class QuickWheelController : MonoBehaviour
{
    public Canvas quickWheelCanvas;  // Drag your Canvas with the quick wheel UI here
    public Button[] wheelButtons;  // Array to store buttons for each slot (Warp, Stasis, Gun, Sword)
    public KeyCode quickWheelKey = KeyCode.LeftAlt;  // Key to trigger quick wheel (Left Alt)
    public float wheelAngleOffset = 90f;  // Angle between slots
    private bool isQuickWheelActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // Make the quick wheel inactive initially
        quickWheelCanvas.gameObject.SetActive(false);

        // Add listeners to handle the button clicks for each slot
        wheelButtons[0].onClick.AddListener(() => SelectSlot(0)); // Warp
        wheelButtons[1].onClick.AddListener(() => SelectSlot(1)); // Stasis
        wheelButtons[2].onClick.AddListener(() => SelectSlot(2)); // Gun
        wheelButtons[3].onClick.AddListener(() => SelectSlot(3)); // Sword
    }

    // Update is called once per frame
    void Update()
    {
        HandleQuickWheelInput();
    }

    private void HandleQuickWheelInput()
    {
        // Check if Left Alt is held and the canvas is not active
        if (Input.GetKey(quickWheelKey) && !isQuickWheelActive)
        {
            Debug.Log("Left Alt Pressed");  // This should show in the console
            isQuickWheelActive = true;

            // Activate the QuickWheelCanvas and make it visible
            quickWheelCanvas.gameObject.SetActive(true);  // Make the canvas visible
            Debug.Log("QuickWheelCanvas Activated: " + quickWheelCanvas.gameObject.activeSelf);  // Log to confirm activation

            // Slow the game time (time will pass slower while Left Alt is held)
            Time.timeScale = 0.2f;  // Adjust this value as needed (0.2 is 20% normal speed)
            Debug.Log("Time Slowed: " + Time.timeScale);  // Optional debug to confirm time slowing
        }
        // Check if Left Alt is released and the canvas is active
        else if (!Input.GetKey(quickWheelKey) && isQuickWheelActive)
        {
            isQuickWheelActive = false;

            // Deactivate the QuickWheelCanvas when Left Alt is released
            quickWheelCanvas.gameObject.SetActive(false);  // Hide the canvas
            Debug.Log("QuickWheelCanvas Deactivated: " + quickWheelCanvas.gameObject.activeSelf);  // Log to confirm deactivation

            // Resume normal game time speed
            Time.timeScale = 1f;  // Return to normal time speed
            Debug.Log("Time Resumed: " + Time.timeScale);  // Optional debug to confirm time resumption
        }
    }



    private void SelectSlot(int slotIndex)
    {
        // Here, you can trigger the logic for each ability/weapon selection
        Debug.Log("Selected Slot: " + wheelButtons[slotIndex].name);

        // You can also add effects like activating the ability or switching weapons.
        // For example:
        switch (slotIndex)
        {
            case 0: // Warp
                // Trigger Warp ability logic here
                break;
            case 1: // Stasis
                // Trigger Stasis ability logic here
                break;
            case 2: // Gun
                // Equip Gun here
                break;
            case 3: // Sword
                // Equip Sword here
                break;
        }
    }

}
    