using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class MapCollisionSystem : SystemBase
{
    Entity prefabDestructable;
    Entity prefabEdgeMap;


    public void CreateCollisionMap(int[,,] map, bool inverted)
    {
        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
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

        Debug.Log("DSFSDFSDF");

        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                if (inverted)
                {
                    if (map[w, h, 0] == 1)
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

    protected override void OnUpdate()
    {
        return;
    }
}
