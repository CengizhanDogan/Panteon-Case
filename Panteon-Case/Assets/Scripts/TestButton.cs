using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField] private Building building;
    
    public void ButtonPress()
    {
        GameObject buildingObject = new GameObject(building.buildingName);
        buildingObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        building.CreateObjects(buildingObject.transform);
    }
}
