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
            Cancel();

            Corner corner = new Corner(coll, transform);

            CheckIfMoving(lastGrid, corner);

            // Directly follow mouse position
            Vector3 followPos = InputManager.MousePosition;

            // Currently clamp is for flag to prevent be placed at absurd distances 
            if (DoClamp(anchor))
            {
                followPos.x = Mathf.Clamp(followPos.x, anchor.x - 3, anchor.x + 4);
                followPos.y = Mathf.Clamp(followPos.y, anchor.y - 3, anchor.y + 4);
            }

            transform.position = followPos;

            if (Input.GetMouseButtonDown(0) && placeable)
            {
                following = false;

                PlaceBuilding(corner);
            }
            yield return null;
        }
    }

    private void Cancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ObjectPooler.Instance.DestroyPoolObject(transform.parent.gameObject);
            EventManager.OnCancel.Invoke();
        }
    }

    private void CheckIfMoving(Vector3 lastGrid, Corner corner)
    {
        if (lastGrid != corner.ClosestGrid())
        {
            lastGrid = corner.ClosestGrid();
            // If building is a different grid once it was changes its position
            if (unitTransform) unitTransform.position = ClosestCorner(corner);
            EventManager.OnGridMove.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2);
        }
    }

    private void PlaceBuilding(Corner corner)
    {
        transform.position = ClosestCorner(corner);

        EventManager.OnBuildingPlace.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2, 3);

        BuildingBehaviour behaviour = unitTransform.GetComponent<BuildingBehaviour>();

        behaviour.CreateFlag();
        behaviour.ProcessAbility();

        // To set z position to 0
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
