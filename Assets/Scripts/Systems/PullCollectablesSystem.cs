using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct PullCollectablesSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float3 playerPos = new float3(0,0,0);
        float collatableDistance = 0;
        float collectingSpeed = 0;

        foreach ((RefRO<PlayerControl> player, RefRO<LocalTransform> playerTransform, RefRO<PullCollectables> pullCollectables) in 
            SystemAPI.Query<RefRO<PlayerControl>, RefRO<LocalTransform>, RefRO<PullCollectables>>())
        {
            playerPos = playerTransform.ValueRO.Position;
            collectingSpeed = 10;
            collatableDistance = pullCollectables.ValueRO.distance;
        }
        foreach ((RefRO<Expirience> _, RefRW<LocalTransform> expirienceTransform) in SystemAPI.Query<RefRO<Expirience>, RefRW<LocalTransform>>())
        {
            float3 expPos = expirienceTransform.ValueRW.Position;

            if (math.distance(playerPos, expPos) < collatableDistance)
            {
                float3 relativePos = playerPos - expPos;
                quaternion end = quaternion.LookRotation(relativePos, math.up());
                expirienceTransform.ValueRW.Rotation = end;
                var direction = math.mul(expirienceTransform.ValueRO.Rotation, new float3(0f, 0f, 1f));
                expirienceTransform.ValueRW.Position += direction * collectingSpeed * SystemAPI.Time.DeltaTime;
            }
        }
    }
}
