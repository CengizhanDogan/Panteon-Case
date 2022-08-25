using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    public TGridObject[,] GridArray { get; private set; }

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    public int Width { get; private set; }
    public int Height { get; private set; }
    private float cellSize;
    private Vector3 originPosition;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        Width = width;
        Height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        GridArray = new TGridObject[width, height];

        for (int x = 0; x < GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < GridArray.GetLength(1); y++)
            {
                GridArray[x, y] = createGridObject(this, x, y);
            }
        }
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

    public void TriggerGridObjectChanged(int x, int y)
    {
        if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            GridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return GridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public Vector2 GetPosition(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetWorldPosition(x, y);
    }
}
