using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int[,] GridArray { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }
    private float cellSize;
    private Vector3 originPosition;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, GameObject gridImage, GridManager manager)
    {
        Width = width;
        Height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        GridArray = new int[width, height];
        this.cellSize = cellSize;
    }

    public Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = (Mathf.RoundToInt(worldPosition.x) - Mathf.RoundToInt(originPosition.x));
        y = (Mathf.RoundToInt(worldPosition.y) - Mathf.RoundToInt(originPosition.y));
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            GridArray[x, y] = value;
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return GridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public Vector2 GetPosition(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetWorldPosition(x, y);
    }
}
