using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Raycast ray;

    void Start()
    {
        ray = new Raycast();
    }
    private void Update()
    {
        ray.SelectObject();
    }
}
