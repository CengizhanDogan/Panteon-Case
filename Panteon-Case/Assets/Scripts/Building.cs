using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "GameEntities/Building", order = 1)]
public class Building : Unit
{
    public List<Unit> craftingList = new List<Unit>();
    public override void CreateObjects(Vector2 spawnPos)
    {
        base.CreateObjects(spawnPos);

        gfx.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        behaviour.isStaticObject = true;

        GameObject mover = new GameObject("Mover");

        var moverCollider = mover.AddComponent<BoxCollider2D>();
        var buildingMovement = mover.AddComponent<BuildingMovement>();
        
        moverCollider.size = gfx.GetComponent<BoxCollider2D>().size;
        
        mover.transform.SetParent(gfx.transform);
        mover.transform.localPosition = Vector3.zero;

        buildingMovement.gfxTransform = gfx.transform;
        buildingMovement.gfxCollider = gfx.GetComponent<Collider2D>();

        EventManager.OnBuildingBought.Invoke(gfx.GetComponent<SpriteRenderer>(), cost);
    }
}