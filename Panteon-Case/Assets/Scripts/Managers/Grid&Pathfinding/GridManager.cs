using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        [HideInInspector] public static Grid<PathNode> MainGrid { get; private set; }

        public int gridWidth;
        public int gridHeight;

        [SerializeField] private GameObject gridImage;

        public static GridManager Instance { get; private set; }
        private void Awake()
        {
            #region Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            #endregion

            // Creates grid with given values
            MainGrid = new Grid<PathNode>(gridWidth, gridHeight, 1f, new Vector3(-gridWidth, -gridHeight) * .5f, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

            CreateGridVisual();
        }

        private void CreateGridVisual()
        {
            // Creates visual at every grid

            for (int x = 0; x < MainGrid.GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < MainGrid.GridArray.GetLength(1); y++)
                {
                    Instantiate(gridImage, MainGrid.GetWorldPosition(x, y), Quaternion.identity, transform);
                }
            }
        }
    }
}
