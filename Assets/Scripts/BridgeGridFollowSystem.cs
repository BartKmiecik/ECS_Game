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
        gridController = GameObject.FindAnyObjectByType<GridController>().GetComponent<GridController>();
    }

    protected override void OnUpdate()
    {

    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        Cell result = gridController.curFlowField.GetCellFromWorldPos(worldPos);
        return result;
    }
}
