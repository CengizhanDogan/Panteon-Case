using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    public Entity entity;
    private void OnMouseDown()
    {
        EventManager.OnSelectionEvent.Invoke(entity);
    }
}
