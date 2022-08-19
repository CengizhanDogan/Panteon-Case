using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building", order = 1)]
public class Building : ScriptableObject
{
    public string buildingName;
    public Sprite visual;
    public Material material;
    public int cost;

    public void CreateObjects(Transform transform)
    {
        GameObject gfx = new GameObject("GFX");
        
        var gfxRenderer = gfx.AddComponent<SpriteRenderer>();

        gfxRenderer.sprite = visual;
        gfxRenderer.material = material;
        gfxRenderer.sortingOrder = 3;

        var gfxCollider = gfx.AddComponent<BoxCollider>();

        gfx.transform.SetParent(transform);
        gfx.transform.localPosition = Vector3.zero;

        GameObject mover = new GameObject("Mover");
        
        var moverCollider = mover.AddComponent<BoxCollider>();
        var buildingMovement = mover.AddComponent<BuildingMovement>();
        
        moverCollider.size = gfxCollider.size;
        
        mover.transform.SetParent(transform);
        mover.transform.localPosition = Vector3.zero;

        buildingMovement.gfxTransform = gfx.transform;
        buildingMovement.gfxCollider = gfx.GetComponent<Collider>();

        EventManager.OnBuildingBought.Invoke(gfxRenderer, transform, cost);
    }
}
