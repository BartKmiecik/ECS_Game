using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
