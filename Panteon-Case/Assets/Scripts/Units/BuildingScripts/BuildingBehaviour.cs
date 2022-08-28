using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Factory;
using System;

public class BuildingBehaviour : UnitBehaviour
{
    public Transform flagTransform;
    public void ProcessAbility()
    {
        UnitAbility ability = UnitAbilityFactory.GetUnit(unit.unitName);

        if (ability == null) return;

        ability.Process();
    }

    public void CreateFlag()
    {
        BuildingObject building = unit as BuildingObject;
        if (building.productionList.Count > 0)
        {
            ObjectPooler.Instance.SpawnFromPool("Flag", InputManager.MousePosition, Quaternion.identity, out var gameObject);
            EventManager.OnBuildingBought.Invoke(gameObject.GetComponent<SpriteRenderer>(), 0);
            gameObject.GetComponentInChildren<IPlaceableBuilding>().Move();
            Vector2 myPos = transform.position;
            gameObject.GetComponentInChildren<IClampable>().CheckClamp = true;
            gameObject.GetComponentInChildren<IClampable>().DoClamp(myPos);
            flagTransform = gameObject.transform;
        }
    }
}
