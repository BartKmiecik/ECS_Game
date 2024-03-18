using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Physics;
using UnityEditor;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellSize;
    public FlowField curFlowField;
    public Transform indicator;

    public void InitializeFlowField()
    {
        curFlowField = new FlowField(cellSize, gridSize);
        curFlowField.CreateGrid();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitializeFlowField();
            curFlowField.CreateCostField();

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            UnityEngine.RaycastHit hit;
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                Debug.Log($"RAY trans POS: {objectHit} pos {objectHit.position}");
            }
            Cell destinationCell = curFlowField.GetCellFromWorldPos(worldMousePos);
            indicator.position = worldMousePos;
            
            curFlowField.CreateDestinationCell(destinationCell);
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
                        Vector3 pos = new Vector3(curFlowField.grid[x, y].wordPos.x, 0, curFlowField.grid[x, y].wordPos.z);
                        Handles.Label(pos, curFlowField.grid[x, y].bestCost.ToString());
                    }
                }
            }
        }

    }
}
