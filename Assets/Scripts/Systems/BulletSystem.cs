using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BulletSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
    }

    public void OnUpdate(ref SystemState state) {

        foreach ((RefRO<Bullet> bullet, RefRW<LocalTransform> localTransform) in SystemAPI.Query<RefRO<Bullet>, RefRW<LocalTransform>>())
        {
            var direction = math.mul(localTransform.ValueRO.Rotation, new float3(0f, 0f, 1f));
            var speed = bullet.ValueRO.speed;
            localTransform.ValueRW.Position += direction * speed * SystemAPI.Time.DeltaTime;
        }
    }
}
