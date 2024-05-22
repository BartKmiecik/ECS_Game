using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TerrainUtils;

[BurstCompile]
public partial class ECSFlowFieldSystem : SystemBase
{
    CellularAutomataMapGenerator mapGenerator;
    private int terrainMask;
    private bool hasIncreasedCost = false;

    protected override void OnCreate()
    {
        mapGenerator = GameObject.FindAnyObjectByType<CellularAutomataMapGenerator>();
        Debug.Log("Found map generator");
    }

    protected override void OnUpdate()
    {

    }

    public void CreateCostField()
    {
        for (int w = 0; w < mapGenerator.Map.GetLength(0); w++)
        {
            for(int h = 0; h < mapGenerator.Map.GetLength(1); h++)
            {
                hasIncreasedCost = false;
            }
        }

        /*foreach (int curCell in mapGenerator.Map)
        {
            
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
        }*/
    }
}
