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

[Flags]
public enum CollisionLayers
{
    Player = 1 << 1,
    Enemies = 1 << 2
}

[BurstCompile]
public partial struct AutoTargetingSystem : ISystem
{
    PhysicsWorldSingleton world;
    float3 zeros;
    float radius;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
        zeros = new float3(0, 0, 0);
        radius = 15;
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<Player> player, RefRO<LocalTransform> LocalTransform) in SystemAPI.Query<RefRO<Player>, RefRO<LocalTransform>>())
        {
            NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
            world = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            world.SphereCastAll(LocalTransform.ValueRO.Position, radius, zeros, 1, ref hits, new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayers.Player,
                CollidesWith = (uint)CollisionLayers.Enemies,
            });
            List<float3> targetsList = new List<float3>();
            foreach (ColliderCastHit hit in hits)
            {
                ComponentLookup<LocalTransform> localTransform = SystemAPI.GetComponentLookup<LocalTransform>();
                try
                {
                    float3 enemyPos = new float3(localTransform.GetRefRW(hit.Entity).ValueRO.Position);
                    targetsList.Add(enemyPos);
                } catch (Exception ex)
                {
                    Debug.Log($"{ex.Message} - Enemy destroyed in meantime");
                }    
            }
            if (targetsList.Count > 0) 
            {
                targetsList.Sort((p1, p2) => math.distance(LocalTransform.ValueRO.Position, p1).CompareTo(math.distance(LocalTransform.ValueRO.Position, p2)));
            }
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AutomaticTargetHolder>().SetTargetsPos(targetsList);
            break;
        }
    }
}

