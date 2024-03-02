using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct DamageSystem : ISystem
{
    int toHit;
    EntityCommandBuffer ecb;
    EntityManager em;
    public void OnCreate(ref SystemState state)
    {
        toHit = 5;
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        /*var space = Input.GetKeyDown("space");
        ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        if (space){
            int i = 0;
            foreach ((RefRW<Health> health, Entity entity) in SystemAPI.Query<RefRW<Health>>().WithEntityAccess())
            {
                while (health.ValueRW.isAlive && i < toHit)
                {
                    ++i;
                    health.ValueRW.health -= 5;
                    if (health.ValueRW.health <= 0)
                    {
                        
                        health.ValueRW.isAlive = false;
                        ecb.DestroyEntity(entity);
                    }
                }
            }
        }*/
        ecb.Playback(em);
        ecb.Dispose();
    }

/*    public partial struct IDamageJob : IJobEntity
    {
        public void Execute()
        {
            
        }
    }*/
}
