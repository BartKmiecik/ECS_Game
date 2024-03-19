using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
	public GridDebug gridDebug;

	public bool shouldFocusOnObject = false;
	public Transform objectToFocus;
    public int frameDeley;
    private int curFrame = 0;

    private void InitializeFlowField()
	{
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
		gridDebug.SetFlowField(curFlowField);
	}

	private void Update()
	{
		if (!shouldFocusOnObject)
		{
            if (Input.GetMouseButtonDown(0))
            {
                InitializeFlowField();

                curFlowField.CreateCostField();

                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
                Cell destinationCell = curFlowField.GetCellFromWorldPos(worldMousePos);
                curFlowField.CreateIntegrationField(destinationCell);

                curFlowField.CreateFlowField();

                gridDebug.DrawFlowField();
            }
        }
        else
        {
            if (curFrame >= 0)
            {
                InitializeFlowField();

                curFlowField.CreateCostField();

                Cell destinationCell = curFlowField.GetCellFromWorldPos(objectToFocus.position);
                curFlowField.CreateIntegrationField(destinationCell);

                curFlowField.CreateFlowField();

                gridDebug.DrawFlowField();
                curFrame = 0;
            }
            curFrame += 1;
        }

	}
}
