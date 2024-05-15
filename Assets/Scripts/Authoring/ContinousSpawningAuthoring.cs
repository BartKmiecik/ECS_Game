using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.IO.Archive;
using UnityEngine;

public class ContinousSpawningAuthoring : MonoBehaviour
{
    public List<GameObject> prefabKindsToSpawne = new List<GameObject>();
    public float radius;
    public float cooldown;
    public float reduceCooldown;
    public int reduceStep;
    public float rotOffset;

    public class Baker : Baker<ContinousSpawningAuthoring>
    {
        public override void Bake(ContinousSpawningAuthoring authoring)
        {
            var buffer = AddBuffer<Spawnable>().Reinterpret<Entity>();
            foreach (var prefab in authoring.prefabKindsToSpawne)
                buffer.Add(GetEntity(prefab, TransformUsageFlags.Dynamic));
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ContinousSpawn
            {
                radius = authoring.radius,
                cooldown = authoring.cooldown,
                reduceCooldown = authoring.reduceCooldown,
                currentCooldown = 0,
                rotOffset = authoring.rotOffset,
                currentRotOffset = 0,
                reduceStep = authoring.reduceStep,
            });
        }
    }
}

public partial struct ContinousSpawn : IComponentData
{
    public float radius;
    public float cooldown;
    public float reduceCooldown;
    public int reduceStep;
    public float currentCooldown;
    public float rotOffset;
    public float currentRotOffset;
}