using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float3 offset = float3.zero;

    private EntityManager manager;

    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Start()
    {
        var entities = manager.GetAllEntities(Allocator.Temp);
        foreach (var entity in entities)
        {
            if (manager.HasComponent<Player>(entity))
            {
                entityToFollow = entity;
                break;
            }
        }
    }

    private void LateUpdate()
    {
        if (entityToFollow != null)
        {
            LocalTransform entPos = manager.GetComponentData<LocalTransform>(entityToFollow);
            transform.position = entPos.Position + offset;
        }
    }
}
