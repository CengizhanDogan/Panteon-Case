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
        
        // Current Grid Object is Path Nodes for Pathfinding for soldier units
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
        // Gets grids world position by adding offset like cell size and origin position to given x and y
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        // Getting grid by world position
        x = (Mathf.RoundToInt(worldPosition.x) - Mathf.RoundToInt(originPosition.x));
        y = (Mathf.RoundToInt(worldPosition.y) - Mathf.RoundToInt(originPosition.y));
    }

    public Vector2 GetPosition(Vector3 worldPosition)
    {
        // Finds grid at given world position and finds that grids position
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetWorldPosition(x, y);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        // Currently gets Path Node at given grid
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
        // Gets grid object with world position
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

}
