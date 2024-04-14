using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float lerpSpeed = 10;

    private EntityManager manager;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
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
        if (manager.Exists(entityToFollow))
        {
            LocalTransform entPos = manager.GetComponentData<LocalTransform>(entityToFollow);
            transform.position = Vector3.Lerp(transform.position, entPos.Position, Time.deltaTime * lerpSpeed);
        }
        else
        {
            Init();
        }
    }
}
