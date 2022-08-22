using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "GameEntities/Building", order = 1)]
public class Building : Entity
{
    public List<Unit> unitList = new List<Unit>();
    public override void CreateObjects()
    {
        base.CreateObjects();

        GameObject mover = new GameObject("Mover");

        mover.layer = LayerMask.NameToLayer("Input");

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
