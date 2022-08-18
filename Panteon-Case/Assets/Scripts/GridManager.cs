using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
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

    [HideInInspector] public Grid grid;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private GameObject gridImage;
    void Start()
    {
        grid = new Grid(gridWidth, gridHeight, 1f, new Vector3(-gridWidth, -gridHeight) * .5f, gridImage, this);
    }

}
