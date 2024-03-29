using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateBefore(typeof(PhysicsSimulationGroup))]
public partial struct SimpleSpawnSystem : ISystem
{
    public bool paused;
    int amount;
    Entity prefab;
    EntityManager manager;
    public void OnCreate(ref SystemState state)
    {
        amount = -1;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (RefRO<SimpleEnemySpawner> spawner in SystemAPI.Query<RefRO<SimpleEnemySpawner>>())
            {
                amount = spawner.ValueRO.amount;
                prefab = spawner.ValueRO.entityToSpawn;
            }
            for (int i = 0; i < amount; i++)
            {
                Entity spawnedEntity = manager.Instantiate(prefab);
                manager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = new Unity.Mathematics.float3(2 * i, 1f, 0),
                    Rotation = Quaternion.identity,
                    Scale = 1
                });
                manager.SetComponentData(spawnedEntity, new PhysicsVelocity
                {
                    Linear = 0
                });
            }
            amount = 0;
        }
        /*if (amount == -1)
        {
            foreach (RefRO<SimpleEnemySpawner> spawner in SystemAPI.Query<RefRO<SimpleEnemySpawner>>())
            {
                amount = spawner.ValueRO.amount;
                prefab = spawner.ValueRO.entityToSpawn;
            }

        }
        for (int i = 0; i < amount; i++)
        {
            Entity spawnedEntity = manager.Instantiate(prefab);
            manager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = new Unity.Mathematics.float3(2 * i, 2f , 0),
                Rotation = Quaternion.identity,
                Scale = 1
            });
            manager.SetComponentData(spawnedEntity, new PhysicsVelocity
            {
                Linear = 0
            });
        }
        amount = 0;*/
        /*ISimpleSpawnJob simpleSpawnJob = new ISimpleSpawnJob
        {
            _simpleEnemySpawner = simpleEnemySpawner,
            _manager = manager
        };
        simpleSpawnJob.Schedule();*/
    }

    public partial struct ISimpleSpawnJob : IJobEntity
    {
        public SimpleEnemySpawner _simpleEnemySpawner;
        public EntityManager _manager;
        public void Execute()
        {
            var amount = _simpleEnemySpawner.amount;
            var prefab = _simpleEnemySpawner.entityToSpawn;
            for (int i = 0; i < amount; i++)
            {
                Entity spawnedEntity = _manager.Instantiate(prefab);
                _manager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = new Unity.Mathematics.float3(2 * i, 0, 0),
                    Scale = 1
                });
            }
        }
    }
}
