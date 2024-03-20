using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

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
    private Cell destinationCell = null;
/*
    private void OnEnable()
    {
        GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
        GC.Collect();
    }

    private void OnDisable()
    {
        GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
    }*/


    private void InitializeFlowField()
	{
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
		gridDebug.SetFlowField(curFlowField);
	}

        
	private void Update()
	{
        if (curFrame >= 0)
        {
            InitializeFlowField();

            curFlowField.CreateCostField();

            destinationCell = curFlowField.GetCellFromWorldPos(objectToFocus.position);
            curFlowField.CreateIntegrationField(destinationCell);

            curFlowField.CreateFlowField();
            curFrame = 0;
        }
        curFrame += 1;
    }
}
