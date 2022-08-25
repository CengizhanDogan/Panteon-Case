using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast
{
    public void SelectObject()
    {
        if (!ClickedOnObject) return;

        if (Hit(MousePos2D).transform.TryGetComponent(out UnitBehaviour unit))
        {
            unit.GetSelected();
        }
    }

    public GameObject SelectedObject()
    {
        if (!ClickedOnObject) return null;

        if (Hit(MousePos2D).transform.TryGetComponent(out UnitBehaviour unit))
        {
            if (!unit.isStaticObject)
                return unit.gameObject;
        }

        return null;
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

                EventManager.OnDeselectEvent.Invoke();
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
