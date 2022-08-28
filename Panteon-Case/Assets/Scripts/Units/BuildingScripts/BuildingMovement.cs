using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class BuildingMovement : MonoBehaviour, IPlaceableBuilding, IClampable
{
    [HideInInspector] public Transform unitTransform;
    [HideInInspector] public Collider2D unitCollider;
    [HideInInspector] public BuildingObject building;

    private Collider2D coll;
    private Vector2 anchor;

    [HideInInspector] public bool placeable;
    [HideInInspector] public bool clamp;

    public bool CheckClamp { get; set; }

    public bool DoClamp(Vector2 anchor)
    {
        this.anchor = anchor;
        if (CheckClamp)
            return Vector2.Distance(InputManager.MousePosition, this.anchor) > 4;
        else return false;
    }

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
        StartCoroutine(Follow(true));
    }

    private void SetColliders()
    {
        coll.enabled = true;
        unitCollider.enabled = true;
    }

    public virtual IEnumerator Follow(bool following)
    {
        Vector3 lastGrid = Vector3.zero;
        while (following)
        {
            Corner corner = new Corner(coll, transform);

            CheckIfMoving(lastGrid, corner);

            if (!DoClamp(anchor))
                transform.position = InputManager.MousePosition;

            if (Input.GetMouseButtonDown(0) && placeable)
            {
                following = false;

                PlaceBuilding(corner);

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

    private void PlaceBuilding(Corner corner)
    {
        transform.position = ClosestCorner(corner);
        EventManager.OnBuildingPlace.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2, 3);

        unitTransform.GetComponent<BuildingBehaviour>().CreateFlag();

        unitTransform.position *= (Vector2.up + Vector2.right);

        coll.enabled = false;
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
