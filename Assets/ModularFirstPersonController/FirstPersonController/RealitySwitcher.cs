using UnityEngine;
using UnityEngine.UI;


public class RealitySwitcher : MonoBehaviour
{

    public GameObject reality1;
    public GameObject reality2;
    public Material skybox1;
    public Material skybox2;
    private bool isReality1Active = true;
    private bool isReality2Active = false;

    // Update is called once per frame
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
        isReality1Active = !isReality1Active;

        reality1.SetActive(isReality1Active);
        reality2.SetActive(isReality2Active);

        //SetLayer(reality1, isReality1Active ? "Reality1" : "Ignore Raycast");
        //Setlayer(reality2, isReality2Active ? "Ignore Raycast" : "Reality2");

        RenderSettings.skybox = isReality1Active ? skybox1 : skybox2;
        DynamicGI.UpdateEnvironment();

        Debug.Log("Reality switched");

    }

    void Setlayer(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);

        foreach(Transform child in obj.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}
