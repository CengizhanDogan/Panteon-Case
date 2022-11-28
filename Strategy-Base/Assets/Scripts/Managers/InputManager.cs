using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private RaycastManager ray;

        private static Camera cam;
        public static Vector3 MousePosition => cam.ScreenToWorldPoint(Input.mousePosition);

        private void Awake()
        {
            cam = Camera.main;
            ray = new RaycastManager();
        }
        private void Update()
        {
            ray.SelectObject();
        }
    }
}
