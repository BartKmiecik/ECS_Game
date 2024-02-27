using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SimpleSpawnSystem : ISystem
{
    int amount;
    Entity prefab;
    EntityManager manager;
    public void OnCreate(ref SystemState state)
    {
        Debug.Log("OnCreate!");
        amount = -1;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (RefRO<SimpleEnemySpawner> spawner in SystemAPI.Query<RefRO<SimpleEnemySpawner>>())
        {
            amount = spawner.ValueRO.amount;
            prefab = spawner.ValueRO.entityToSpawn;
            Debug.Log("MANAGER");
            Debug.Log(amount + " ON Create");
        }
        Debug.Log(manager);
    }

    public void OnUpdate(ref SystemState state)
    {
        Debug.Log("OnUpdate!");
        /*        Debug.Log(amount);
                Debug.Log(prefab);*/
        if (amount == -1)
        {
            foreach (RefRO<SimpleEnemySpawner> spawner in SystemAPI.Query<RefRO<SimpleEnemySpawner>>())
            {
                amount = spawner.ValueRO.amount;
                prefab = spawner.ValueRO.entityToSpawn;
                Debug.Log("Update");
                Debug.Log(amount + " ON Create");
            }
        }
        for (int i = 0; i < amount; i++)
        {
            Debug.Log("Entity Spawned");
            Entity spawnedEntity = manager.Instantiate(prefab);
            manager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = new Unity.Mathematics.float3(2 * i, 0, 0),
                Scale = 1
            });
        }
        amount = 0;
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