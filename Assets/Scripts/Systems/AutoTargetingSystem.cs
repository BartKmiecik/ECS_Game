using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

[BurstCompile]
public partial struct AutoTargetingSystem : ISystem
{
    PhysicsWorld world;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
        Debug.Log("Start");
        world = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
    }

    public void OnUpdate(ref SystemState state)
    {
        Debug.Log("SMTH");
        Raycast(new float3(0, 0, 0), new float3(0, 0, 10), out var hit);
    }

    private bool Raycast(float3 rayStart , float3 rayEnd, out RaycastHit raycast)
    {
        try
        {
            RaycastInput rayInput = new RaycastInput
            {
                Start = rayStart,
                End = rayEnd,
            };
            var hit = world.CastRay(rayInput, out raycast);
            Debug.Log(hit);
            return hit;
        }
        catch {
            world = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
            raycast = new RaycastHit();
        }
        return false;
    }
}
