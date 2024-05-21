using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct GridMapSpawnerSystem : ISystem
{
    public bool paused;
    private int prefabCounter;
    EntityManager manager;
    private float deg2rad;
    float space;
    int rotationCounter;
    DynamicBuffer<Spawnable> dynamicBuffer;
    float timer;

    public void OnCreate(ref SystemState state)
    {
        timer = 0;
        state.RequireForUpdate<GridMapSpawner>();
        prefabCounter = 0;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        paused = false;
        deg2rad = Mathf.Deg2Rad;
        space = 30;
        rotationCounter = 0;
        dynamicBuffer = new DynamicBuffer<Spawnable>();
    }

    public void OnUpdate(ref SystemState state)
    {
        timer += SystemAPI.Time.DeltaTime;
        /*foreach ((RefRW<GridMapSpawner> spawner, Entity entity) in SystemAPI.Query<RefRW<GridMapSpawner>>().WithEntityAccess())
        {
            if (spawner.ValueRO.currentCooldown > spawner.ValueRO.cooldown)
            {
                spawner.ValueRW.currentCooldown -= spawner.ValueRO.cooldown;
                dynamicBuffer = manager.GetBuffer<Spawnable>(entity);
                int bufferLenght = dynamicBuffer.Length;

                Entity[] prefabs = new Entity[bufferLenght];
                for (int i = 0; i < dynamicBuffer.Length; i++)
                {
                    prefabs[i] = dynamicBuffer[i].prefab;
                }

                int x = UnityEngine.Random.Range(0, 100);
                int y = UnityEngine.Random.Range(0, 100);

                Entity spawnedEntity = manager.Instantiate(prefabs[prefabCounter++]);
                manager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = new float3(x, 3f, y),
                    Rotation = Quaternion.identity,
                    Scale = 1
                });
                manager.SetComponentData(spawnedEntity, new PhysicsVelocity
                {
                    Linear = 0
                });
                if (prefabCounter >= prefabs.Length)
                {
                    prefabCounter = 0;
                }
                Debug.Log($"Spawn enemy at x:{x} y:{y}");
            }
            spawner.ValueRW.currentCooldown += SystemAPI.Time.DeltaTime;
        }*/
    }
}
