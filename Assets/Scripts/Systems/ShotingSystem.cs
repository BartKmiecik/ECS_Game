using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct ShotingSystem : ISystem
{
    EntityManager manager;
    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        var space = Input.GetKeyDown("space");
        if (space)
        {
            foreach ((RefRO<PlayerShooting> player, RefRO<LocalToWorld> localTransform) in SystemAPI.Query<RefRO<PlayerShooting>, RefRO<LocalToWorld>>())
            {
                Entity prefab = player.ValueRO.buletPrefab;
                var temp = localTransform.ValueRO.Value;
                Entity spawnedEntity = manager.Instantiate(prefab);
                manager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = temp.Translation(),
                    Rotation = temp.Rotation(),
                    Scale = 1
                });
            }
        } 
    }
}
