using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct DashingEnemySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state) 
    {
        foreach ((RefRO<LookAt> lookAt, RefRW<LocalTransform> localTransform, RefRW<DashingEnemy> dashingEnemy, RefRW<PhysicsVelocity> physic) in
            SystemAPI.Query<RefRO<LookAt>, RefRW<LocalTransform>, RefRW<DashingEnemy>, RefRW<PhysicsVelocity>>())
        {
            if (dashingEnemy.ValueRO.currentCooldown <= 0)
            {
                float force = dashingEnemy.ValueRO.force;
                float3 relativePos = math.normalizesafe(lookAt.ValueRO.target - localTransform.ValueRO.Position);
                dashingEnemy.ValueRW.currentCooldown = dashingEnemy.ValueRO.cooldown;
                physic.ValueRW.Linear = relativePos * force;
            }
            dashingEnemy.ValueRW.currentCooldown -= SystemAPI.Time.DeltaTime;
        }
    }
}
