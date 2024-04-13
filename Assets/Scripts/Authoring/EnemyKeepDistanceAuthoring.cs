using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemyKeepDistanceAuthoring : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
    public float speed;

    public class Baker : Baker<EnemyKeepDistanceAuthoring>
    {
        public override void Bake(EnemyKeepDistanceAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyKeepDistance
            {
                minDistnace = authoring.minDistance,
                maxDistnace = authoring.maxDistance,
                speed = authoring.speed,
            });
        }
    }
}

public partial struct EnemyKeepDistance : IComponentData
{
    public float minDistnace;
    public float maxDistnace;
    public float speed;
}