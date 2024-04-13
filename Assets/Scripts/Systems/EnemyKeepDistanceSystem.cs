using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemyKeepDistanceSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        float3 playerPos = new float3();
        foreach ((RefRO<Player> _, RefRO<LocalTransform> playerTransform) in SystemAPI.Query<RefRO<Player>, RefRO<LocalTransform>>())
        {
            playerPos = playerTransform.ValueRO.Position;
        }


        foreach ((RefRO<EnemyKeepDistance> keepDistance, RefRO<LocalTransform> localTransform, RefRW<EnemySimpleShooting> enemyShoting, RefRW<PhysicsVelocity> physic) in 
            SystemAPI.Query<RefRO<EnemyKeepDistance>, RefRO<LocalTransform>, RefRW<EnemySimpleShooting>, RefRW<PhysicsVelocity>>())
        {
            if (enemyShoting.ValueRO.currentCooldown > enemyShoting.ValueRO.cooldown / 2)
            {
                return;
            }

            float dist = Vector3.Distance(playerPos, localTransform.ValueRO.Position);

            float3 physicMovement = math.normalizesafe(playerPos - localTransform.ValueRO.Position);

            if (dist < keepDistance.ValueRO.minDistnace)
            {
                physicMovement.y = 0;
                physicMovement.x *= keepDistance.ValueRO.speed;
                physicMovement.z *= keepDistance.ValueRO.speed;
                physic.ValueRW.Linear = -physicMovement;
                enemyShoting.ValueRW.currentCooldown = 0;
                return;
            }
            if (dist > keepDistance.ValueRO.maxDistnace)
            {
                physicMovement.y = 0;
                physicMovement.x *= keepDistance.ValueRO.speed;
                physicMovement.z *= keepDistance.ValueRO.speed;
                physic.ValueRW.Linear = physicMovement;
                enemyShoting.ValueRW.currentCooldown = 0;
                return;
            }
        }
    }
}
