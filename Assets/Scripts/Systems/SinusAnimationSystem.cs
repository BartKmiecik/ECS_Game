using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SinusAnimationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SinusAnimation> ();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<SinusAnimation> sinus, RefRW<LocalTransform> localTrasform ) in 
            SystemAPI.Query<RefRO<SinusAnimation>, RefRW<LocalTransform>>())
        {
            float3 localPos = sinus.ValueRO.localPos;
            localPos.y += (float)math.sin(SystemAPI.Time.ElapsedTime * sinus.ValueRO.speed) * sinus.ValueRO.magnitude;
            localTrasform.ValueRW.Position = math.lerp(localTrasform.ValueRO.Position, localPos, 0.5f);
        }
    }
}
