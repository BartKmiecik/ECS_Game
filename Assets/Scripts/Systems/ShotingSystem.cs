using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct ShotingSystem : ISystem
{
    public bool paused;
    EntityManager manager;
    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        foreach ((RefRW<PlayerShooting> player, RefRO<LocalToWorld> localTransform) in SystemAPI.Query<RefRW<PlayerShooting>, RefRO<LocalToWorld>>())
        {
            var space = Input.GetMouseButton(0);
            if (space)
            {
                if (player.ValueRO.currentCooldown >= player.ValueRO.cooldown)
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
                    player.ValueRW.currentCooldown = 0;
                }
            }  
            if (player.ValueRO.currentCooldown < player.ValueRO.cooldown)
            {
                player.ValueRW.currentCooldown += SystemAPI.Time.DeltaTime;
            }
        }
    }
}
