using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct ShotingSystem : ISystem
{
    EntityManager manager;
    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        var space = Input.GetMouseButton(0);
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
