using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager
{
    public GameObject SelectedObject { get; private set; }
    public void SelectObject()
    {
        if (!ClickedOnObject) return;

        if (Hit(MousePos2D).transform.TryGetComponent(out UnitBehaviour unit))
        {
            SelectedObject = unit.gameObject;
            Debug.Log(unit.gameObject);
            unit.GetSelected();
        }
    }

    private bool ClickedOnObject
    {
        get
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Hit(MousePos2D).collider != null)
                {
                    return true;
                }

                EventManager.OnDeselectEvent.Invoke(true);
            }

            return false;
        }
    }

    private RaycastHit2D Hit(Vector2 mousePos2D)
    {
        return Physics2D.Raycast(mousePos2D, Vector2.zero);
    }

    private Vector2 MousePos2D
    {
        get
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mousePos.x, mousePos.y);
        }
    }
}
