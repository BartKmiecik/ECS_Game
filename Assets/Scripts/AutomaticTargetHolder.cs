using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public partial class AutomaticTargetHolder : SystemBase
{
    private List<float3> targets_pos = new List<float3>();
    
    protected override void OnUpdate()
    {
        return;
    }

    public void SetTargetsPos(List<float3> targets_pos)
    {
        this.targets_pos.Clear();
        this.targets_pos.AddRange(targets_pos);
    }

    public float3 GetClosestTarget()
    {
        if (targets_pos.Count == 0)
            return float3.zero;
        return targets_pos[0];
    }
}
