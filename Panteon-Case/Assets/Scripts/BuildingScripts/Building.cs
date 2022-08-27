using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "GameEntities/Building", order = 1)]
public class Building : Unit
{
    public Building flag;
    [HideInInspector] public GameObject flagObject;
    public List<Unit> productionList = new List<Unit>();
    public override void CreateObjects(Vector2 spawnPos, int sortingLayer, out GameObject unit, bool move, GameObject flag)
    {
        base.CreateObjects(spawnPos, sortingLayer, out unit, false, null);

        gfx.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        behaviour.isStaticObject = true;

        GameObject mover = new GameObject("Mover");

        var moverCollider = mover.AddComponent<BoxCollider2D>();
        BuildingMovement buildingMovement;

        if (unitName == "Flag") buildingMovement = mover.AddComponent<FlagMovement>();
        else buildingMovement = mover.AddComponent<BuildingMovement>();

        moverCollider.size = gfx.GetComponent<BoxCollider2D>().size;

        mover.transform.SetParent(gfx.transform);
        mover.transform.localPosition = Vector3.zero;

        buildingMovement.gfxTransform = gfx.transform;
        buildingMovement.gfxCollider = gfx.GetComponent<Collider2D>();
        buildingMovement.building = this;

        EventManager.OnBuildingBought.Invoke(gfx.GetComponent<SpriteRenderer>(), cost);

    }
}