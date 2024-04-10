using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using RaycastHit = Unity.Physics.RaycastHit;


[BurstCompile]
public partial struct Raycasttest : ISystem
{
    PhysicsWorldSingleton world;
    float3 zeros;
    float radius;
    float offset;
    float currentOffset;
    float3 tz;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
        zeros = new float3(0, 0, 0);
        radius = 15;
        offset = 15;
        currentOffset = 0;
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<Player> player, RefRO<LocalTransform> LocalTransform) in SystemAPI.Query<RefRO<Player>, RefRO<LocalTransform>>())
        {
            //NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
            world = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            /*            world.SphereCastAll(LocalTransform.ValueRO.Position, radius, zeros, 1, ref hits, new CollisionFilter
                        {
                            BelongsTo = (uint)CollisionLayers.Player,
                            CollidesWith = (uint)CollisionLayers.Enemies,
                        });*/
            RaycastInput rayInput = new RaycastInput {
                Start = LocalTransform.ValueRO.Position,
                End = LocalTransform.ValueRO.Forward() * radius,
                Filter = new CollisionFilter()
                {
                    BelongsTo = 1 << 1,
                    CollidesWith = 1 << 2, 
                    GroupIndex = 0
                }

            };

            tz = LocalTransform.ValueRO.Position;

            RaycastHit hit = new RaycastHit();
            bool haveHit = world.CastRay(rayInput, out hit);

            List<float3> targetsList = new List<float3>();
            if (haveHit)
            {
                ComponentLookup<LocalTransform> localTransform = SystemAPI.GetComponentLookup<LocalTransform>();
                float3 enemyPos = new float3(localTransform.GetRefRW(hit.Entity).ValueRO.Position);
                Debug.Log($"Target at {localTransform}");
            }
            break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float spacing = 360 / 16;
        for (int i = 0; i < 16; i++)
        {
            float x = radius * Mathf.Sin((spacing * i + currentOffset) * Mathf.Deg2Rad);
            float y = radius * Mathf.Cos((spacing * i + currentOffset) * Mathf.Deg2Rad);
            Gizmos.DrawLine(tz, new Vector3(x, 1, y));
        }
        currentOffset += offset;
        if (currentOffset >= 360)
        {
            currentOffset = 0;
        }
    }
}
