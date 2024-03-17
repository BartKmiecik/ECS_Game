using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellSize;
    public FlowField curFlowField;

    public void InitializeFlowField()
    {
        Debug.Log("Initialize FLow field");
        curFlowField = new FlowField(cellSize, gridSize);
        curFlowField.CreateGrid();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            InitializeFlowField();
            curFlowField.CreateCostField();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (curFlowField != null)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 center = new Vector3(cellSize * 2 * x + cellSize, 0, cellSize * 2 * y + cellSize);
                    Vector3 size = Vector3.one * cellSize * 2;
                    Gizmos.DrawWireCube(center, size);
                }
            }
            if (curFlowField.hasCostField)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        Handles.Label(curFlowField.grid[x, y].wordPos, curFlowField.grid[x, y].cost.ToString());
                    }
                }
            }
        }

    }
}
