using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(Destructable))]
[BurstCompile]
public partial struct DropableSystem : ISystem
{
    EntityManager entityManager;
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Dropable>();
        state.RequireForUpdate<Destructable>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<Destructable> destructable, RefRO<Dropable> dropable, RefRO<LocalToWorld> localTransform) in
            SystemAPI.Query<RefRO<Destructable>, RefRO<Dropable>, RefRO<LocalToWorld>>())
        {
            if (destructable.ValueRO.shouldBeDestroyed)
            {
                Entity prefab = dropable.ValueRO.entityToDrop;
                Entity spawnedEntity = entityManager.Instantiate(prefab);
                Debug.Log("Spawning exp");
                entityManager.SetComponentData(spawnedEntity, new LocalTransform
                {
                    Position = localTransform.ValueRO.Position,
                    Scale = 1
                });
            }
        }
    }
}
