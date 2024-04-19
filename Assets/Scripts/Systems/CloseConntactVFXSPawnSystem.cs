using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct CloseConntactVFXSPawnSystem : ISystem
{
    float3 defaultPos;
    EntityManager manager;

    public void OnCreate(ref SystemState state)
    {
        defaultPos = new float3(999.0f, 999.0f, 999.0f);
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<EnemyTag> enemyTag, RefRO<LocalTransform> localTransform) in 
            SystemAPI.Query<RefRW<EnemyTag>, RefRO<LocalTransform>>()) {
            if (math.all(enemyTag.ValueRO.conntactPoint != defaultPos) && enemyTag.ValueRO.attackVFX != null)
            {
                Entity spawnedEntity = manager.Instantiate(enemyTag.ValueRO.attackVFX);
                manager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = enemyTag.ValueRO.conntactPoint,
                    Rotation = localTransform.ValueRO.Rotation,
                    Scale = 1
                });
                enemyTag.ValueRW.conntactPoint = defaultPos;
            }
        }
    }
}
