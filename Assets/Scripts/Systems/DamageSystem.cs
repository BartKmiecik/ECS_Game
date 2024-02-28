using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct DamageSystem : ISystem
{
    int toHit;
    public void OnCreate(ref SystemState state)
    {
        toHit = 5;
    }

    public void OnUpdate(ref SystemState state)
    {
        var space = Input.GetKeyDown("space");
        if (space){
            int i = 0;
            foreach (RefRW<Health> health in SystemAPI.Query<RefRW<Health>>())
            {
                while (health.ValueRW.isAlive && i < toHit)
                {
                    ++i;
                    health.ValueRW.health -= 5;
                    if (health.ValueRW.health <= 0)
                    {
                        health.ValueRW.isAlive = false;
                    }
                }
            }
        }
    }

/*    public partial struct IDamageJob : IJobEntity
    {
        public void Execute()
        {
            
        }
    }*/
}
