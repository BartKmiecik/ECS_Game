using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;
using Unity.Entities;
using RaycastHit = Unity.Physics.RaycastHit;
using Unity.Mathematics;

public partial class MapCollisionSystem : SystemBase
{
    EntityManager manager;
    Entity prefabDestructable;
    Entity prefabEdgeMap;
    CellularAutomataMapGenerator cellularAutomataMapGenerator;
    Entity playerEntity;
    Entity[,] collisionBlocks;

    public void CreateCollisionMap(int[,,] map, bool inverted, CellularAutomataMapGenerator mapGenerator)
    {
        cellularAutomataMapGenerator = mapGenerator;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entities = manager.GetAllEntities(Allocator.Temp);
        foreach (var entity in entities)
        {
            if (manager.HasComponent<ColliderHolder>(entity))
            {
                ComponentLookup<ColliderHolder> colliderHolder = SystemAPI.GetComponentLookup<ColliderHolder>();
                prefabDestructable = colliderHolder.GetRefRO(entity).ValueRO.collisionPrefabDestructalbe;
                prefabEdgeMap = colliderHolder.GetRefRO(entity).ValueRO.collisionPrefabEdgeMap;
                Debug.Log($"PREFAAAAAA: {prefabDestructable}");
                break;
            }
        }
        foreach (var entity in entities)
        {
            if (manager.HasComponent<Player>(entity))
            {
                playerEntity = entity;
                break;
            }
        }
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        collisionBlocks = new Entity[width, height];

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                if (inverted)
                {
                    if (map[w, h, 0] == 1)
                    {
                        Entity spawnedEntity = manager.Instantiate(prefabEdgeMap);
                        collisionBlocks[w, h] = spawnedEntity;
                        manager.SetComponentData(spawnedEntity, new LocalTransform
                        {
                            Position = new Unity.Mathematics.float3(w, 2, h),
                            Rotation = Quaternion.identity,
                            Scale = 1
                        });
                    }
                }
                else
                {
                    if (map[w, h, 0] == 0)
                    {
                        Entity spawnedEntity = manager.Instantiate(prefabEdgeMap);
                        manager.SetComponentData(spawnedEntity, new LocalTransform
                        {
                            Position = new Unity.Mathematics.float3(w, 2, h),
                            Rotation = Quaternion.identity,
                            Scale = 1
                        });
                    }
                }

                if (inverted)
                {
                    if (map[w, h, 1] == 0)
                    {
                        Entity spawnedEntity = manager.Instantiate(prefabDestructable);
                        manager.SetComponentData(spawnedEntity, new LocalTransform
                        {
                            Position = new Unity.Mathematics.float3(w, 2, h),
                            Rotation = Quaternion.identity,
                            Scale = 1
                        });
                    }
                }
                else
                {
                    if (map[w, h, 1] == 1)
                    {
                        Entity spawnedEntity = manager.Instantiate(prefabDestructable);
                        manager.SetComponentData(spawnedEntity, new LocalTransform
                        {
                            Position = new Unity.Mathematics.float3(w, 2, h),
                            Rotation = Quaternion.identity,
                            Scale = 1
                        });
                    }
                }
            }
        }
    }

    public void DestroyBlock(Entity block, ComponentLookup<Destructable> destructable)
    {
        LocalTransform entPos = manager.GetComponentData<LocalTransform>(block);
        destructable.GetRefRW(block).ValueRW.shouldBeDestroyed = true;
        cellularAutomataMapGenerator.RemoveWall((int)entPos.Position.x, (int)entPos.Position.y);
    }

    protected override void OnUpdate()
    {
        return;
    }
}
