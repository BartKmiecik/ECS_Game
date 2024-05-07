using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public enum SpawningType
{
    Line,
    Circle
}
public class PeriodicSpawnerAuthoring : MonoBehaviour
{
    public List<GameObject> prefabKindsToSpawne = new List<GameObject>();
    public int maxPrefabSpawn;
    public SpawningType spawningType;
    public float radius;
    public bool automaticSpawn;
    public float cooldown;
    public float rotOffset;

    public class Baker : Baker<PeriodicSpawnerAuthoring>
    {
        public override void Bake(PeriodicSpawnerAuthoring authoring)
        {
            var buffer = AddBuffer<Spawnable>().Reinterpret<Entity>();
            foreach (var prefab in authoring.prefabKindsToSpawne)
                buffer.Add(GetEntity(prefab, TransformUsageFlags.Dynamic));
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PeriodicSpawner
            {
                maxEntitesToSpawn = authoring.maxPrefabSpawn,
                spawningType = authoring.spawningType,
                radius = authoring.radius,
                automaticSpawn = authoring.automaticSpawn,
                cooldown = authoring.cooldown,
                currentCooldown = 0,
                rotOffset = authoring.rotOffset,
                currentRotOffset = 0,
            });
        }
    }
}

public struct Spawnable : IBufferElementData
{
    public Entity prefab;
}

public partial struct PeriodicSpawner : IComponentData
{
    public int maxEntitesToSpawn;
    public SpawningType spawningType;
    public float radius;
    public bool automaticSpawn;
    public float cooldown;
    public float currentCooldown;
    public float rotOffset;
    public float currentRotOffset;
}
