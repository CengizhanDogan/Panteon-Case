using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    public UnitObject unit;

    public virtual void GetSelected()
    {
        EventManager.OnSelectionEvent.Invoke(this, transform);
    }
}
