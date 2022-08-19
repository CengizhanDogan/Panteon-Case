using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner
{
    [HideInInspector] public Vector3 CornerPos { get; private set; }

    private Transform buildingTransform;
    private Collider buildingCollider;

    public Corner(Collider coll, Transform transform)
    {
        buildingCollider = coll;
        buildingTransform = transform;

        PlaceCorner();
    }

    private void PlaceCorner()
    {
        Vector3 cornerPos = buildingTransform.position;

        cornerPos.y -= buildingCollider.bounds.extents.y;
        cornerPos.x -= buildingCollider.bounds.extents.x;

        CornerPos = cornerPos;
    }

    public Vector3 ClosestGrid()
    {
        float distance = Mathf.Infinity;
        Vector3 closestGrid = Vector3.zero;

        foreach (Vector3 grid in RelativeGridPositions())
        {
            float closestCorner = Vector3.Distance(CornerPos, grid);

            if (closestCorner < distance)
            {
                distance = closestCorner;

                closestGrid = grid;
            }
        }

        return closestGrid;
    }
    private List<Vector3> RelativeGridPositions()
    {
        Grid grid = GridManager.Instance.MainGrid;

        List<Vector3> grids = new List<Vector3>();

        int gridX, gridY;
        grid.GetXY(this.CornerPos, out gridX, out gridY);

        grids.Add(grid.GetPosition(this.CornerPos));
        grids.Add(grid.GetWorldPosition(gridX + 1, gridY));
        grids.Add(grid.GetWorldPosition(gridX, gridY + 1));
        grids.Add(grid.GetWorldPosition(gridX + 1, gridY + 1));

        return grids;
    }
}
