using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

public partial struct ShotingSystem : ISystem
{
    EntityCommandBuffer ecb;
    EntityManager manager;
    Entity prefab;
    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        var space = Input.GetKeyDown("space");
        if (space)
        {
            ecb = new EntityCommandBuffer(Allocator.TempJob);
            foreach ((RefRO<Player> player, RefRO<LocalToWorld> transform) in SystemAPI.Query<RefRO<Player>, RefRO<LocalToWorld>>())
            {
                Entity spawnedEntity = manager.Instantiate(prefab);
                manager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = new Unity.Mathematics.float3(2 * i, 0, 0),
                    Scale = 1
                });
            }
        } 
    }
}
