using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    void Update()
    {
        if (Screen.currentResolution.width <= Screen.currentResolution.height)
        {
            Debug.Log("ChangeUI");
        }
    }
}
