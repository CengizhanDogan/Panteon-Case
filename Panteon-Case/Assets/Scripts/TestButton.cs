using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField] private Building building;
    
    public void ButtonPress()
    {
        building.CreateObjects();
    }
}
