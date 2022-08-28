using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class BuildingMovement : MonoBehaviour, IPlaceableBuilding
{
    [HideInInspector] public bool placeable;

    private Collider2D coll;

    [HideInInspector] public Transform unitTransform;
    [HideInInspector] public Collider2D unitCollider;
    [HideInInspector] public BuildingObject building;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        EventManager.OnPlaceableEvent.AddListener(() => placeable = true);
        EventManager.OnUnavailableEvent.AddListener(() => placeable = false);
    }

    private void OnDisable()
    {
        EventManager.OnPlaceableEvent.RemoveListener(() => placeable = true);
        EventManager.OnUnavailableEvent.RemoveListener(() => placeable = false);
    }
    public void Move()
    {
        SetColliders();
        StartCoroutine(Follow(true, 3));
    }

    private void SetColliders()
    {
        coll.enabled = true;
        unitCollider.enabled = true;
    }

    public virtual IEnumerator Follow(bool following, int sortingOrder)
    {
        Vector3 lastGrid = Vector3.zero;
        while (following)
        {
            Corner corner = new Corner(coll, transform);

            CheckIfMoving(lastGrid, corner);
            
            transform.position = InputManager.MousePosition;

            if (Input.GetMouseButtonDown(0) && placeable)
            {
                following = false;

                PlaceBuilding(corner, sortingOrder);
                
            }
            yield return null;
        }
    }


    private void CheckIfMoving(Vector3 lastGrid, Corner corner)
    {
        if (lastGrid != corner.ClosestGrid())
        {
            lastGrid = corner.ClosestGrid();
            if (unitTransform) unitTransform.position = ClosestCorner(corner);
            EventManager.OnGridMove.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2);
        }
    }

    private void PlaceBuilding(Corner corner, int sortingOrder)
    {
        transform.position = ClosestCorner(corner);
        EventManager.OnBuildingPlace.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2, sortingOrder);

        unitTransform.GetComponent<BuildingBehaviour>().CreateFlag();

        coll.enabled = false;
        if (sortingOrder == 1) unitCollider.enabled = false;
    }

    private Vector3 ClosestCorner(Corner corner)
    {
        Vector3 gridPos = Vector3.zero;

        gridPos = corner.ClosestGrid();
        gridPos.y += coll.bounds.extents.y;
        gridPos.x += coll.bounds.extents.x;

        return gridPos;
    }
}
