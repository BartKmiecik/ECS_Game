using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    public class Baker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Movement
            {
                value = new float3 (UnityEngine.Random.Range(-2,2), 0.5f, UnityEngine.Random.Range(-2, 2))
            });
        }
    }
}

public struct Movement : IComponentData
{
    public float3 value;
}
