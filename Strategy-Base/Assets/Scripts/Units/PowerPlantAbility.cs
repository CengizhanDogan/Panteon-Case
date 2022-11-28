using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantAbility : UnitAbility
{
    public override string Name => "PowerPlant";
    

    public Vector3 pos;
    public override void Process()
    {
        EventManager.OnPowerGeneration.Invoke(1);
    }
}
