using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEditor.PackageManager;
using UnityEngine;

public enum SpawningType
{
    Line,
    Circle
}
public class BetterSpawnerAuthoring: MonoBehaviour
{
    public List<GameObject> prefabKindsToSpawne = new List<GameObject>();
    public int maxPrefabSpawn;
    public SpawningType spawningType;
    public float radius;

    public class Baker : Baker<BetterSpawnerAuthoring>
    {
        public override void Bake(BetterSpawnerAuthoring authoring)
        {
            var buffer = AddBuffer<Spawnable>().Reinterpret<Entity>();
            foreach (var prefab in authoring.prefabKindsToSpawne)
                buffer.Add(GetEntity(prefab, TransformUsageFlags.Dynamic));
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BetterSpawner
            {
                maxEntitesToSpawn = authoring.maxPrefabSpawn,
                spawningType = authoring.spawningType,
                radius = authoring.radius,
            });
        }
    }
}

public struct Spawnable : IBufferElementData
{
    public Entity prefab;
}

public partial struct BetterSpawner : IComponentData
{
    public int maxEntitesToSpawn;
    public SpawningType spawningType;
    public float radius;
}