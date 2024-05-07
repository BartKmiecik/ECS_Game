using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ContinousSpawnSystem : ISystem
{
    public bool paused;
    private int prefabCounter;
    EntityManager manager;
    private float deg2rad;
    float space;
    int rotationCounter;
    DynamicBuffer<Spawnable> dynamicBuffer;


    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ContinousSpawn>();
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
        foreach ((RefRW<ContinousSpawn> spawner, Entity entity) in SystemAPI.Query<RefRW<ContinousSpawn>>().WithEntityAccess())
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

                float3 playerPos = new float3(0, 0, 0);
                foreach ((RefRO<Player> player, RefRO<LocalTransform> localTransform)
                    in SystemAPI.Query<RefRO<Player>, RefRO<LocalTransform>>())
                {
                    playerPos = localTransform.ValueRO.Position;
                }

                float radius = spawner.ValueRO.radius;
                float x = radius * Mathf.Sin((space * rotationCounter++ + spawner.ValueRO.currentRotOffset) * deg2rad);
                float y = radius * Mathf.Cos((space * rotationCounter++ + spawner.ValueRO.currentRotOffset) * deg2rad);

                rotationCounter = rotationCounter < 100 ? rotationCounter : 0;

                spawner.ValueRW.currentRotOffset += spawner.ValueRO.rotOffset;

                if (spawner.ValueRO.currentRotOffset >= 360)
                {
                    spawner.ValueRW.currentRotOffset -= 360;
                }

                if (CheckIfBound(ref state, new float3(x, 1f, y) + playerPos))
                {
                    Entity spawnedEntity = manager.Instantiate(prefabs[prefabCounter++]);
                    manager.SetComponentData(spawnedEntity, new LocalTransform
                    {
                        Position = new float3(x, 1f, y) + playerPos,
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
                }
            }
            spawner.ValueRW.currentCooldown += SystemAPI.Time.DeltaTime;
        }
    }

    private bool CheckIfBound(ref SystemState state, float3 potentialPos)
    {
        var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        RaycastInput rayInput = new RaycastInput
        {
            Start = potentialPos,
            End = new float3(potentialPos.x, -20, potentialPos.z),
            Filter = new CollisionFilter()
            {
                BelongsTo = 1 << 2,
                CollidesWith = 1 << 3,
                GroupIndex = 0
            }

        };

        var tz = potentialPos;
        RaycastHit hit = new RaycastHit();
        bool haveHit = world.CastRay(rayInput, out hit);
        List<float3> targetsList = new List<float3>();
        if (haveHit)
        {
            ComponentLookup<LocalTransform> localTransform = SystemAPI.GetComponentLookup<LocalTransform>();
            float3 enemyPos = new float3(localTransform.GetRefRW(hit.Entity).ValueRO.Position);
            return true;
        }
        else
        {
            Debug.Log("Out of bounds");
        }
        return false;
    }
}
