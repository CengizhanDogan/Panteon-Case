using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attribute", menuName = "GameEntities/Attributes", order = 3)]
public class UnitAttributes : ScriptableObject
{
    [HideInInspector] public string unitName;
    public Sprite unitImage;

    public int unitHealth;
    public int unitDamage;
    public int unitSpeed;
}
