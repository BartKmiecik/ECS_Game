using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[BurstCompile]
public partial struct DamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Health>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<Health> health, RefRW<Destructable> destructable) in SystemAPI.Query<RefRW<Health>, RefRW<Destructable>>())
        {
            if (health.ValueRW.currentHealth <= 0)
            {
                health.ValueRW.isAlive = false;
                destructable.ValueRW.shouldBeDestroyed = true;
            }
        }
    }

}
