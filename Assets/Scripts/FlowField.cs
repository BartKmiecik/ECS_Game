using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField 
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    private float cellDiameter;

    public bool hasCostField = false;

    public FlowField(float cellRaius, Vector2Int gridSize)
    {
        this.gridSize = gridSize;
        this.cellRadius = cellRaius;
        cellDiameter = cellRaius * 2;
    }

    public void CreateGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 wordPos = new Vector3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);
                grid[x, y] = new Cell(wordPos, new Vector2Int(x, y));
            }
        }
    }

    public void CreateCostField()
    {
        Vector3 cellHalfExtents = Vector3.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impossible", "RoughTerrain");
        foreach (Cell curCell in grid)
        {
            Collider[] obstacles = Physics.OverlapBox(curCell.wordPos, cellHalfExtents, Quaternion.identity, terrainMask);
            
            bool hasIncreasedCost = false;
            foreach (Collider obstacle in obstacles) 
            {
                Debug.Log(obstacle.name);
                //TODO Hardcoded value
                if (obstacle.gameObject.layer == 6)
                {
                    curCell.IncreaseCost(255);
                    continue;
                } 
                else if (!hasIncreasedCost && obstacle.gameObject.layer == 7)
                {
                    curCell.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }
        hasCostField = true;
    }
}
