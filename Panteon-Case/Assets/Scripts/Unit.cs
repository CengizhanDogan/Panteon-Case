using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Unit", menuName = "GameEntities/Unit", order = 2)]
public class Unit : ScriptableObject
{
    public string unitName;
    public Sprite visual;
    public Material material;
    public int cost;

    public UnitAttributes attributes;
    public UnitBehaviour behaviour;

    [HideInInspector] public GameObject gfx;

    private void Awake()
    {
        attributes.unitName = unitName;
    }
    public virtual void CreateObjects(Vector2 spawnPos)
    {
        gfx = new GameObject(unitName);

        gfx.transform.position = spawnPos;

        var gfxRenderer = gfx.AddComponent<SpriteRenderer>();

        gfxRenderer.sprite = visual;
        gfxRenderer.material = material;
        gfxRenderer.sortingOrder = 3;

        gfx.AddComponent<BoxCollider2D>();

        behaviour = gfx.AddComponent<UnitBehaviour>();

        behaviour.unit = this;
    }
}
