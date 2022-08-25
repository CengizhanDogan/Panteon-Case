using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [HideInInspector] public Grid<PathNode> MainGrid { get; private set; }

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private GameObject gridImage;

    private Pathfinding pathfinding;

    private Vector2 objectGrid;
    private GameObject moveObject;

    #region Singleton
    public static GridManager Instance { get; private set; }
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
        MainGrid = new Grid<PathNode>(gridWidth, gridHeight, 1f, new Vector3(-gridWidth, -gridHeight) * .5f, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

        for (int x = 0; x < MainGrid.GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < MainGrid.GridArray.GetLength(1); y++)
            {
                Instantiate(gridImage, MainGrid.GetWorldPosition(x, y), Quaternion.identity, transform);
            }
        }

        pathfinding = new Pathfinding(gridWidth, gridHeight);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Raycast ray = new Raycast();

            moveObject = ray.SelectedObject();
            if (!moveObject) return;
            MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
            objectGrid = new Vector2(x, y);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!moveObject) return;
            if (moveObject.GetComponent<UnitBehaviour>().isStaticObject) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetXY(new Vector2(mousePos.x, mousePos.y), out int x, out int y);
            List<PathNode> path = pathfinding.FindPath((int)objectGrid.x, (int)objectGrid.y, x, y);
            StartCoroutine(Walk(path));
        }
    }

    IEnumerator Walk(List<PathNode> path)
    {
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                moveObject.transform.position = MainGrid.GetWorldPosition(path[i].x, path[i].y);
                yield return new WaitForSeconds(0.1f);
            }

            MainGrid.GetXY(moveObject.transform.position, out var x, out var y);
            objectGrid = new Vector2(x, y);
        }
    }
}
