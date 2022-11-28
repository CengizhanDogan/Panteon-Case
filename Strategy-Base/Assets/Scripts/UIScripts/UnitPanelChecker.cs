using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPanelChecker : MonoBehaviour
{
    [SerializeField] private GameObject verticalPanel;
    [SerializeField] private GameObject horizantalPanel;

    private void Start()
    {
        // Checks resolution od the screen
        if (Screen.width > Screen.height)
        {
            verticalPanel.SetActive(true);
        }
        else
        {
            horizantalPanel.SetActive(true);
        }
    }
}
