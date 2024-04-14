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

        foreach ((RefRO<PlayerControl> player, RefRO<LocalTransform> playerTransform, RefRO<PullCollectables> pullCollectables) in 
            SystemAPI.Query<RefRO<PlayerControl>, RefRO<LocalTransform>, RefRO<PullCollectables>>())
        {
            playerPos = playerTransform.ValueRO.Position;
            collatableDistance = pullCollectables.ValueRO.distance;
        }
        foreach ((RefRW<Expirience> expireince, RefRW<LocalTransform> expirienceTransform) in SystemAPI.Query<RefRW<Expirience>, RefRW<LocalTransform>>())
        {
            float3 expPos = expirienceTransform.ValueRW.Position;

            if (math.distance(playerPos, expPos) < collatableDistance)
            {
                expireince.ValueRW.shouldBeCollected = true;
            }

            if (expireince.ValueRO.shouldBeCollected)
            {
                float3 relativePos = playerPos - expPos;
                quaternion end = quaternion.LookRotation(relativePos, math.up());
                expirienceTransform.ValueRW.Rotation = end;
                var direction = math.mul(expirienceTransform.ValueRO.Rotation, new float3(0f, 0f, 1f));
                expirienceTransform.ValueRW.Position += direction * expireince.ValueRO.collectingFlySpeed * SystemAPI.Time.DeltaTime;
            }
        }
    }
}
