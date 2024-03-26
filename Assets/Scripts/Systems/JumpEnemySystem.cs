using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct JumpEnemySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state) 
    {
        foreach ((RefRW<JumpEnemy> jumpEnemy, RefRO<LocalTransform> localTransform, RefRW<PhysicsVelocity> physic, RefRO<LookAt> target) in 
            SystemAPI.Query<RefRW<JumpEnemy>, RefRO<LocalTransform>, RefRW<PhysicsVelocity>, RefRO<LookAt>>())
        {
            if (jumpEnemy.ValueRO.currentCooldown <= 0)
            {
                float force = jumpEnemy.ValueRO.force;
                float3 relativePos = math.normalizesafe(target.ValueRO.target - localTransform.ValueRO.Position);
                relativePos.y = 1;
                jumpEnemy.ValueRW.currentCooldown = jumpEnemy.ValueRO.cooldown;
                physic.ValueRW.Linear = relativePos * force;
            }
            jumpEnemy.ValueRW.currentCooldown -= SystemAPI.Time.DeltaTime;
        }
    }
}
