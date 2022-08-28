using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAbility
{
    public abstract string Name { get; }

    public abstract void Process();
}
