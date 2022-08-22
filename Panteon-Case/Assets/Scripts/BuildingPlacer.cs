using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private List<RectTransform> imgRectTransforms = new List<RectTransform>();

    private void OnEnable()
    {
        EventManager.OnGridMove.AddListener(CheckIfPlaceable);
        EventManager.OnBuildingPlace.AddListener(PlaceOnGrid);
    }
    private void OnDisable()
    {
        EventManager.OnGridMove.RemoveListener(CheckIfPlaceable);
        EventManager.OnBuildingPlace.RemoveListener(PlaceOnGrid);
    }

    private void CheckIfPlaceable(BuildingMovement building, Vector2 size)
    {
        if (CheckIfOnCanvas())
        {
            EventManager.OnUnavailableEvent.Invoke();
            return;
        }

        Corner corner = new Corner(building.gfxCollider, building.gfxTransform);
        int gridX, gridY;
        GridManager.Instance.MainGrid.GetXY(corner.CornerPos + Vector3.one * 0.1f, out gridX, out gridY);

        for (int x = gridX; x < gridX + size.x; x++)
        {
            for (int y = gridY; y < gridY + size.y; y++)
            {
                int gridValue = GridManager.Instance.MainGrid.GetValue(x, y);

                if (gridValue != 0)
                {
                    EventManager.OnUnavailableEvent.Invoke();
                    return;
                }
            }
        }

        EventManager.OnPlaceableEvent.Invoke();

    }

    private void PlaceOnGrid(BuildingMovement building, Vector2 size)
    {
        Corner corner = new Corner(building.gfxCollider, building.gfxTransform);
        int gridX, gridY;
        GridManager.Instance.MainGrid.GetXY(corner.CornerPos + Vector3.one * 0.1f, out gridX, out gridY);

        for (int x = gridX; x < gridX + size.x; x++)
        {
            for (int y = gridY; y < gridY + size.y; y++)
            {
                GridManager.Instance.MainGrid.SetValue(x, y, 99);
            }
        }
    }

    private bool CheckIfOnCanvas()
    {
        foreach (var imgRectTransform in imgRectTransforms)
        {
            Vector2 localMousePosition = imgRectTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (imgRectTransform.rect.Contains(localMousePosition))
            {
                return true;
            }
        }

        return false;
    }
}
