using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FlowField 
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    private float cellDiameter;
    public bool hasCostField = false;
    public Cell destination;

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

    public void CreateDestinationCell(Cell cell)
    {
        destination = cell;
        destination.cost = 0;
        destination.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();

        cellsToCheck.Enqueue(destination);

        while (cellsToCheck.Count > 0)
        {
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> curNeighbors = GetNeighborsCells(curCell.gridIdx, GridDirection.CardinalDirections);
            foreach (Cell neighbor in curNeighbors)
            {
                if(neighbor.cost == byte.MaxValue)
                {
                    continue;
                }
                if(neighbor.cost + curCell.bestCost < neighbor.bestCost)
                {
                    neighbor.bestCost = (ushort)(neighbor.cost + curCell.bestCost);
                    cellsToCheck.Enqueue(neighbor);
                }
            }
        }
    }

    private List<Cell> GetNeighborsCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighbors = new List<Cell>();

        foreach (Vector2Int curDirection in directions)
        {
            Cell newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
            if (newNeighbor != null)
            {
                neighbors.Add(newNeighbor);
            }
        }
        return neighbors;
    }

    private Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }
        else
        {
            return grid[finalPos.x, finalPos.y];
        }
    }

    public Cell GetCellFromWorldPos(Vector3 wordPos)
    {
        float percentX = wordPos.x / (gridSize.x * cellDiameter);
        float percentY = wordPos.z / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return grid[x, y];
    }

}
