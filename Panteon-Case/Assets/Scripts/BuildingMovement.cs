using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : MonoBehaviour
{
    private bool following;
    [HideInInspector] public bool placeable;

    private Collider coll;

    [HideInInspector] public Transform gfxTransform;
    [HideInInspector] public Collider gfxCollider;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    private void Start()
    {
        following = true;
        StartCoroutine(Follow());
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
    private IEnumerator Follow()
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

            Vector2 followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position =  followPos;

            if (Input.GetMouseButtonDown(0) && placeable)
            {
                transform.position = ClosestCorner(corner);
                following = false;
                EventManager.OnBuildingPlace.Invoke(this, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y) * 2);
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
