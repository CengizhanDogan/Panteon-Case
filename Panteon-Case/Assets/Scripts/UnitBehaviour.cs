using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    public Unit unit;
    public bool isStaticObject;

    public void GetSelected()
    {
        EventManager.OnSelectionEvent.Invoke(unit);
    }
}
