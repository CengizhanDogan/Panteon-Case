using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "GameEntities/Unit", order = 2)]
public class UnitObject : ScriptableObject
{
    public string unitName;
    public Sprite visual;
    public Material material;
    public int cost;

    public UnitAttributes attributes;
    public UnitBehaviour behaviour;

    [HideInInspector] public GameObject unitGameObject;

    private void Awake()
    {
        attributes.unitName = unitName;
    }
}
