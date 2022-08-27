using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitMover : MonoBehaviour
{
    private Pathfinding pathfinding;

    private Vector2 objectGrid;
    private GameObject moveObject;

    private GridManager grigManager;

    #region Singleton
    public static UnitMover Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    void Start()
    {
        grigManager = GridManager.Instance;
        pathfinding = new Pathfinding(grigManager.gridWidth, grigManager.gridHeight);
    }

    void Update()
    {
        MovementInput();
    }

    public void StartMovement(GameObject moveObject, Transform flag)
    {
        if (!moveObject) return;
        this.moveObject = moveObject;

        grigManager.MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
        Vector2 objectGrid = new Vector2(x, y);

        grigManager.MainGrid.GetXY(flag.position, out var z, out var t);
        List<PathNode> path = pathfinding.FindPath((int)objectGrid.x, (int)objectGrid.y, z, t, false);

        Move(path, 1);
    }

    private void MovementInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Raycast ray = new Raycast();

            moveObject = ray.SelectedObject();
            if (!moveObject) return;
            grigManager.MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
            objectGrid = new Vector2(x, y);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!moveObject) return;
            if (moveObject.GetComponent<UnitBehaviour>().isStaticObject) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetXY(new Vector2(mousePos.x, mousePos.y) + Vector2.up * 0.5f, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath((int)objectGrid.x, (int)objectGrid.y, x, y, true);
            Move(path, 1);
        }
    }

    void Move(List<PathNode> path, int i)
    {
        if (path != null)
        {
            Vector2 gridPos = grigManager.MainGrid.GetWorldPosition(path[i].x, path[i].y);
            moveObject.transform.DOMove(gridPos, 7).SetSpeedBased().SetEase(Ease.Linear).OnUpdate(() =>
            {
                grigManager.MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
                objectGrid = new Vector2(x, y);
            }).OnComplete(() =>
            {
                if (path.Count > i + 2) Move(path, i + 1);
            });
        }
    }
}
