using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    public GameObject radialMenuRoot;

    bool isRadialMenuActive;
    // Start is called before the first frame update
    void Start()
    {
        isRadialMenuActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isRadialMenuActive)
            {
                isRadialMenuActive = !isRadialMenuActive;
                if (isRadialMenuActive)
                {
                    radialMenuRoot.SetActive(true);
                }
                else
                {
                    radialMenuRoot.SetActive(false);
                }
            }
        }
    }
}
