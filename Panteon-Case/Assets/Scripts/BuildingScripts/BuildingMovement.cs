using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : MonoBehaviour
{
    private bool following;
    [HideInInspector] public bool placeable;

    private Collider2D coll;

    [HideInInspector] public Transform gfxTransform;
    [HideInInspector] public Collider2D gfxCollider;
    [HideInInspector] public Building building;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        StartCoroutine(Follow(true, 3));
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
    public virtual IEnumerator Follow(bool following, int sortingOrder)
    {
        Vector3 lastGrid = Vector3.zero;
        while (following)
        {
            Corner corner = new Corner(coll, transform);

            if (lastGrid != corner.ClosestGrid())
            {
                lastGrid = corner.ClosestGrid();
                if (gfxTransform) gfxTransform.position = ClosestCorner(corner);
                EventManager.OnGridMove.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2);
            }

            var followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = followPos;

            if (Input.GetMouseButtonDown(0) && placeable)
            {
                transform.position = ClosestCorner(corner);
                following = false;
                EventManager.OnBuildingPlace.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2, sortingOrder);

                if (building.productionList.Count > 0 && building.flag)
                {
                    building.flag.CreateObjects(Camera.main.ScreenToWorldPoint(Input.mousePosition), 5, out var unit, false, null);
                    building.flagObject = unit;
                }

                Destroy(coll);
                if (sortingOrder == 1) Destroy(gfxCollider);
            }
            yield return null;
        }
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
