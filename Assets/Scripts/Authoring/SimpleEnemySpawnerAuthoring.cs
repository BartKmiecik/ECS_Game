using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SimpleEnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int amount;

    public class Baker : Baker<SimpleEnemySpawnerAuthoring>
    {
        public override void Bake(SimpleEnemySpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SimpleEnemySpawner
            {
                entityToSpawn = GetEntity(authoring.prefabToSpawn, TransformUsageFlags.Dynamic),
                amount = authoring.amount
            });
        }
    }
}

public struct SimpleEnemySpawner : IComponentData
{
    public Entity entityToSpawn;
    public int amount;
}
