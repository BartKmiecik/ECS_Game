using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GridMapSpawnerAuthoring : MonoBehaviour
{
    public List<GameObject> prefabKindsToSpawne = new List<GameObject>();
    public float cooldown;
    public float reduceCooldown;
    public int reduceStep;

    public class Baker : Baker<GridMapSpawnerAuthoring>
    {
        public override void Bake(GridMapSpawnerAuthoring authoring)
        {
            var buffer = AddBuffer<Spawnable>().Reinterpret<Entity>();
            foreach (var prefab in authoring.prefabKindsToSpawne)
                buffer.Add(GetEntity(prefab, TransformUsageFlags.Dynamic));
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GridMapSpawner
            {
                cooldown = authoring.cooldown,
                reduceCooldown = authoring.reduceCooldown,
                currentCooldown = 0,
                currentRotOffset = 0,
                reduceStep = authoring.reduceStep,
            });
        }
    }
}

public partial struct GridMapSpawner : IComponentData
{
    public float cooldown;
    public float reduceCooldown;
    public int reduceStep;
    public float currentCooldown;
    public float currentRotOffset;
}
