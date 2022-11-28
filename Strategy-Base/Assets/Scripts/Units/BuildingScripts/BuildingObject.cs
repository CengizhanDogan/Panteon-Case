using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "GameEntities/Building", order = 1)]
public class BuildingObject : UnitObject
{
    public List<UnitObject> productionList = new List<UnitObject>();

    public bool buyable;
}