using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct DestructableSystem : ISystem
{
    EntityManager entityManager;
    EntityCommandBuffer ecb;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Destructable>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    } 

    public void OnUpdate(ref SystemState state)
    {
        ecb = new EntityCommandBuffer(Allocator.TempJob);
        foreach ((RefRO<Destructable> destructable, Entity entity) in SystemAPI.Query<RefRO<Destructable>>().WithEntityAccess())
        {
            if (destructable.ValueRO.shouldBeDestroyed == true)
            {
                ecb.DestroyEntity(entity);
            }
        }
        ecb.Playback(entityManager);
        ecb.Dispose();
    }

}
