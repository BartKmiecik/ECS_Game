using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public partial struct BetterSpawnSystem : ISystem
{
    public bool paused;
    private int counter;
    int amount;
    EntityManager manager;
    public void OnCreate(ref SystemState state)
    {
        amount = -1;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        paused = false;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("SPAWN");
            Entity[] prefabs = new Entity[3];
            foreach ((RefRO<BetterSpawner> spawner, Entity entity) in SystemAPI.Query<RefRO<BetterSpawner>>().WithEntityAccess())
            {
                amount = spawner.ValueRO.maxEntitesToSpawn;
                var dynamicBuffer = manager.GetBuffer<Spawnable>(entity);
                for (int i = 0; i < dynamicBuffer.Length; i++)
                {
                    prefabs[i] = dynamicBuffer[i].prefab;
                }
            }
            Debug.Log($"Prefabs count {prefabs.Length} and amount {amount} type {prefabs[0].GetType()}");

            /*            amount = BetterSpawner.GetMaxPrefabSpawn();
                        GameObject go = BetterSpawner.GetRandomPrefab();*/
            for (int i = 0; i < amount; i++)
            {
                Entity spawnedEntity = manager.Instantiate(prefabs[counter++]);
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
                if (counter >= prefabs.Length)
                {
                    counter = 0;
                }
            }
            amount = 0;
        }
    }

}
