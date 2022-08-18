using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : MonoBehaviour
{
    bool following;
    Collider coll;

    public Transform indicator;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }
    private void OnMouseDown()
    {
        following = true;
        StartCoroutine(Follow());
    }

    private IEnumerator Follow()
    {
        while (following)
        {
            if (indicator) indicator.position = ClosestCorner();

            Vector2 followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.Lerp(transform.position, followPos, 10 * Time.deltaTime);
            if (Input.GetMouseButtonUp(0))
            {
                transform.position = ClosestCorner();
                following = false;
            }
            yield return null;
        }
    }

    private Vector3 ClosestCorner()
    {
        Vector3 gridPos = Vector3.zero;

        Corner corner = new Corner(coll, transform);

        gridPos = corner.closestGrid;
        gridPos.y -= coll.bounds.extents.y;
        gridPos.x -= coll.bounds.extents.x;

        return gridPos;
    }

    public class Corner
    {
        public Vector3 cornerPos;
        public Vector3 closestGrid;

        public Corner(Collider coll, Transform transform)
        {
            Grid grid = GridManager.Instance.grid;
            List<Vector3> grids = new List<Vector3>();
            float distance = Mathf.Infinity;
            Vector3 cornerPos = transform.position;

            cornerPos.y += coll.bounds.extents.y;
            cornerPos.x += coll.bounds.extents.x;
            this.cornerPos = cornerPos;

            int gridX, gridY;
            grid.GetXY(this.cornerPos, out gridX, out gridY);

            grids.Add(grid.GetPosition(this.cornerPos));
            grids.Add(grid.GetWorldPosition(gridX + 1, gridY));
            grids.Add(grid.GetWorldPosition(gridX, gridY + 1));
            grids.Add(grid.GetWorldPosition(gridX + 1, gridY + 1));

            foreach (Vector3 _grid in grids)
            {
                float closestCorner = Vector3.Distance(this.cornerPos, _grid);

                if (closestCorner < distance)
                {
                    distance = closestCorner;

                    this.closestGrid = _grid;
                }
            }
        }

    }
}
