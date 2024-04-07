using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct EnemySimpleShotingSystem : ISystem
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
        foreach ((RefRW<EnemySimpleShooting> enemy, RefRO<LocalToWorld> localTransform) 
            in SystemAPI.Query<RefRW<EnemySimpleShooting>, RefRO<LocalToWorld>>()) 
        {
            if (enemy.ValueRO.currentCooldown >= enemy.ValueRO.cooldown)
            {
                enemy.ValueRW.currentCooldown = 0;
                Entity prefab = enemy.ValueRO.bulletPrefab;
                var temp = localTransform.ValueRO.Value;
                Entity spawnedEntity = manager.Instantiate(prefab);
                manager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = temp.Translation(),
                    Rotation = temp.Rotation(),
                    Scale = 1
                });
                manager.SetComponentData(spawnedEntity, new EnemyBullet
                {
                    damage_value = manager.GetComponentData<EnemyBullet>(spawnedEntity).damage_value,
                    speed = manager.GetComponentData<EnemyBullet>(spawnedEntity).speed
                });
            }
            
            enemy.ValueRW.currentCooldown += SystemAPI.Time.DeltaTime;
        }
        
    } 
}
