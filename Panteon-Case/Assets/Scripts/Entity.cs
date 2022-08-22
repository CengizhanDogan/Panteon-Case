using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
    public string entityName;
    public Sprite visual;
    public Material material;
    public int cost;

    public UnitAttributes attributes;

    [HideInInspector] public GameObject gfx;

    private void Awake()
    {
        attributes.unitName = entityName;
    }
    public virtual void CreateObjects()
    {
        gfx = new GameObject(entityName);

        var gfxRenderer = gfx.AddComponent<SpriteRenderer>();

        gfxRenderer.sprite = visual;
        gfxRenderer.material = material;
        gfxRenderer.sortingOrder = 3;

        gfx.AddComponent<BoxCollider2D>();
    }
}
