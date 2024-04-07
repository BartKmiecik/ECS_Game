using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct LifespanSystem : ISystem
{
    EntityCommandBuffer ecb;
    EntityManager em;
    public bool paused;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Lifespan>();
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        ecb = new EntityCommandBuffer(Allocator.TempJob);
        foreach ((RefRW<Lifespan> lifespan, Entity entity) in SystemAPI.Query<RefRW<Lifespan>>().WithEntityAccess()) {
            if (lifespan.ValueRW.currentLife > 0)
            {
                lifespan.ValueRW.currentLife -= SystemAPI.Time.DeltaTime;
            }
            else
            {
                ecb.DestroyEntity(entity);
            }
        }
        ecb.Playback(em);
        ecb.Dispose();
    }
}
