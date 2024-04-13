using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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
    private float deg2rad;
    private bool isAutomatic;

    private void Spawn(ref SystemState state)
    {
        int bufferLenght = 0;
        var dynamicBuffer = new DynamicBuffer<Spawnable>();
        SpawningType spawningType = SpawningType.Line;
        float radius = 10;
        float rotOffset = 0;
        foreach ((RefRW<BetterSpawner> spawner, Entity entity) in SystemAPI.Query<RefRW<BetterSpawner>>().WithEntityAccess())
        {
            amount = spawner.ValueRO.maxEntitesToSpawn;
            dynamicBuffer = manager.GetBuffer<Spawnable>(entity);
            bufferLenght = dynamicBuffer.Length;
            spawningType = spawner.ValueRO.spawningType;
            radius = spawner.ValueRO.radius;
            rotOffset = spawner.ValueRO.rotOffset;
        
            Entity[] prefabs = new Entity[bufferLenght];
            for (int i = 0; i < dynamicBuffer.Length; i++)
            {
                prefabs[i] = dynamicBuffer[i].prefab;
            }
    /*        Debug.Log($"Prefabs count {prefabs.Length} and amount {amount} type {prefabs[0].GetType()}");*/
            if (spawningType == SpawningType.Line)
            {
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
            }
            else if (spawningType == SpawningType.Circle)
            {
                float3 playerPos = new float3(0, 0, 0);
                foreach ((RefRO<Player> player, RefRO<LocalTransform> localTransform)
                    in SystemAPI.Query<RefRO<Player>, RefRO<LocalTransform>>())
                {
                    playerPos = localTransform.ValueRO.Position;
                }
                float space = 360 / amount;
                for (int i = 0; i < amount; i++)
                {
                    Entity spawnedEntity = manager.Instantiate(prefabs[counter++]);
                    float x = radius * Mathf.Sin((space * i + spawner.ValueRO.currentRotOffset) * deg2rad);
                    float y = radius * Mathf.Cos((space * i + spawner.ValueRO.currentRotOffset) * deg2rad);

                    spawner.ValueRW.currentRotOffset += spawner.ValueRO.rotOffset;

                    if (spawner.ValueRO.currentRotOffset >= 360)
                    {
                        spawner.ValueRW.currentRotOffset = 0;
                    }

                    manager.SetComponentData(spawnedEntity, new LocalTransform
                    {
                        Position = new Unity.Mathematics.float3(x, 1f, y) + playerPos,
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
            }
            amount = 0;
        }
    }

    public void OnCreate(ref SystemState state)
    {
        amount = -1;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        paused = false;
        deg2rad = Mathf.Deg2Rad;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        foreach (RefRW<BetterSpawner> spawner in SystemAPI.Query<RefRW<BetterSpawner>>())
        {
            isAutomatic = spawner.ValueRO.automaticSpawn;
            if (isAutomatic)
            {
                spawner.ValueRW.currentCooldown += SystemAPI.Time.DeltaTime;
                if (spawner.ValueRW.currentCooldown > spawner.ValueRO.cooldown)
                {
                    spawner.ValueRW.currentCooldown = 0;
                    Spawn(ref state);
                }
            }
            break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Spawn(ref state);
        }
    }

}
