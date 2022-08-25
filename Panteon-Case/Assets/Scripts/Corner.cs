using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner
{
    [HideInInspector] public Vector3 CornerPos { get; private set; }

    private Transform buildingTransform;
    private Collider2D buildingCollider;

    public Corner(Collider2D coll, Transform transform)
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
            Vector3 fixedPos = grid - Vector3.one * 0.5f;

            float closestCorner = Vector3.Distance(CornerPos, fixedPos);

            if (closestCorner < distance)
            {
                distance = closestCorner;

                closestGrid = fixedPos;
            }
        }

        return closestGrid;
    }
    private List<Vector3> RelativeGridPositions()
    {
        Grid<PathNode> grid = GridManager.Instance.MainGrid;

        List<Vector3> grids = new List<Vector3>();

        grid.GetXY(this.CornerPos, out  int gridX, out int gridY);

        grids.Add(grid.GetPosition(this.CornerPos));
        grids.Add(grid.GetWorldPosition(gridX + 1, gridY));
        grids.Add(grid.GetWorldPosition(gridX, gridY + 1));
        grids.Add(grid.GetWorldPosition(gridX + 1, gridY + 1));

        return grids;
    }
}
