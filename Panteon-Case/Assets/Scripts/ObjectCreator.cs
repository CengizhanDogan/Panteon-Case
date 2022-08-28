using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public static class ObjectCreator
{
    public static void CreateObjects(UnitObject unitObject, out GameObject unit)
    {
        unitObject.unitGameObject = new GameObject(unitObject.unitName);

        var gfxRenderer = unitObject.unitGameObject.AddComponent<SpriteRenderer>();

        gfxRenderer.sprite = unitObject.visual;
        gfxRenderer.material = unitObject.material;

        unitObject.unitGameObject.AddComponent<BoxCollider2D>();

        unit = unitObject.unitGameObject;
    }

    public static void CreateMover(UnitObject unitObject, out GameObject unit)
    {
        BuildingBehaviour behaviour = unitObject.unitGameObject.AddComponent<BuildingBehaviour>();
        behaviour.unit = unitObject;

        unitObject.unitGameObject.transform.position = InputManager.MousePosition;

        GameObject mover = new GameObject("Mover");

        var moverCollider = mover.AddComponent<BoxCollider2D>();
        BuildingMovement buildingMovement;

        buildingMovement = mover.AddComponent<BuildingMovement>();

        moverCollider.size = unitObject.unitGameObject.GetComponent<BoxCollider2D>().size;

        mover.transform.SetParent(unitObject.unitGameObject.transform);
        mover.transform.localPosition = Vector3.zero;

        buildingMovement.unitTransform = unitObject.unitGameObject.transform;
        buildingMovement.unitCollider = unitObject.unitGameObject.GetComponent<Collider2D>();
        buildingMovement.building = unitObject as BuildingObject;

        unit = unitObject.unitGameObject;
    }
}
