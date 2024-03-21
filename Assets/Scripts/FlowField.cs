using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
	public Cell[,] grid { get; private set; }
	public Vector2Int gridSize { get; private set; }
	private Vector2Int gridOffset;
	public float cellRadius { get; private set; }
	public Cell destinationCell;
	private float cellDiameter;
    private Vector3 cellHalfExtents;
	private Vector3 worldPos;
	private Vector3 vOne = new Vector3(1,1,1);
	private int terrainMask;
	private bool hasIncreasedCost = false;
	private Collider[] obstacles;
    private List<Cell> curentNeighbors = new List<Cell>();
    private Cell curentCell = null;
    private List<Cell> curNeighbors = new List<Cell>();
    private List<Cell> neighborCells = new List<Cell>();
	private Cell newNeighbor = null;
	private Vector2Int finalPos = new Vector2Int(0,0);
    private float percentX, percentY;
	private int x, y;

    public FlowField(float _cellRadius, Vector2Int _gridSize)
	{
		cellRadius = _cellRadius;
		cellDiameter = cellRadius * 2f;
		gridSize = _gridSize;
		gridOffset = _gridSize / 2;
		cellHalfExtents = vOne * cellRadius;
    }

	public void CreateGrid()
	{
		grid = new Cell[gridSize.x, gridSize.y];

		for (int x = 0; x < gridSize.x; x++)
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				worldPos = new Vector3(cellDiameter * x + cellRadius - gridOffset.x, 0, 
					cellDiameter * y + cellRadius - gridOffset.y);
				grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
			}
		}
	}

	public void CreateCostField()
	{
		terrainMask = LayerMask.GetMask("Impossible", "RoughTerrain");
		foreach (Cell curCell in grid)
		{
			obstacles = Physics.OverlapBox(curCell.worldPos, cellHalfExtents, Quaternion.identity, terrainMask);
			hasIncreasedCost = false;
			foreach (Collider col in obstacles)
			{
				if (col.gameObject.layer == 6)
				{
					curCell.IncreaseCost(255);
					continue;
				}
				else if (!hasIncreasedCost && col.gameObject.layer == 7)
				{
					curCell.IncreaseCost(3);
					hasIncreasedCost = true;
				}
			}
		}
	}

	public void CreateIntegrationField(Cell _destinationCell)
	{
		destinationCell = _destinationCell;

		destinationCell.cost = 0;
		destinationCell.bestCost = 0;

		Queue<Cell> cellsToCheck = new Queue<Cell>();

		cellsToCheck.Enqueue(destinationCell);

		while(cellsToCheck.Count > 0)
		{
			curentCell = cellsToCheck.Dequeue();
            curentNeighbors.Clear();
            curentNeighbors = GetNeighborCells(curentCell.gridIndex, GridDirection.CardinalDirections);
			foreach (Cell curNeighbor in curentNeighbors)
			{
				if (curNeighbor.cost == byte.MaxValue) { continue; }
				if (curNeighbor.cost + curentCell.bestCost < curNeighbor.bestCost)
				{
					curNeighbor.bestCost = (ushort)(curNeighbor.cost + curentCell.bestCost);
					cellsToCheck.Enqueue(curNeighbor);
				}
			}
		}
	}

	public void CreateFlowField()
	{
		foreach(Cell curCell in grid)
		{
			curNeighbors.Clear();
            curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.AllDirections);

			int bestCost = curCell.bestCost;

			foreach(Cell curNeighbor in curNeighbors)
			{
				if(curNeighbor.bestCost < bestCost)
				{
					bestCost = curNeighbor.bestCost;
					curCell.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbor.gridIndex - curCell.gridIndex);
				}
			}
		}
	}

	private List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
	{
		neighborCells.Clear();
		foreach (Vector2Int curDirection in directions)
		{
            newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
			if (newNeighbor != null)
			{
				neighborCells.Add(newNeighbor);
			}
		}
		return neighborCells;
	}

	private Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
	{
		finalPos = orignPos + relativePos;

		if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
		{
			return null;
		}

		else { return grid[finalPos.x, finalPos.y]; }
	}

	public Cell GetCellFromWorldPos(Vector3 worldPos)
	{
		percentX = (worldPos.x + gridOffset.x) / (gridSize.x * cellDiameter);
		percentY = (worldPos.z + gridOffset.y) / (gridSize.y * cellDiameter);

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
		y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
		return grid[x, y];
	}
}
