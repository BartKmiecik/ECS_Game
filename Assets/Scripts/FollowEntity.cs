using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;
using static UnityEngine.GraphicsBuffer;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float lerpSpeed = 10;

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
            transform.position = Vector3.Lerp(transform.position, entPos.Position, Time.deltaTime * lerpSpeed);

        }
    }
}
