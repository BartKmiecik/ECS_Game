using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class BridgeGridFollowSystem : SystemBase
{
    public GridController gridController;

    protected override void OnCreate()
    {
        try
        {
            gridController = GameObject.FindAnyObjectByType<GridController>().GetComponent<GridController>();
        }
        catch
        {
            Debug.Log("Grid controller missing");
        }
    }

    protected override void OnUpdate()
    {

    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        return gridController.curFlowField.GetCellFromWorldPos(worldPos);
    }
}
