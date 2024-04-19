using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTagAuthoring : MonoBehaviour
{
    public int attackValue;
    public GameObject attackVfx;
    public class Baker : Baker<EnemyTagAuthoring>
    {
        public override void Bake(EnemyTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyTag { 
                attackValue = authoring.attackValue,
                attackVFX = GetEntity(authoring.attackVfx, TransformUsageFlags.Dynamic),
                conntactPoint = new float3(999.0f, 999.0f, 999.0f),
            });
        }
    }
}

public struct EnemyTag : IComponentData
{
    public int attackValue;
    public Entity attackVFX;
    public float3 conntactPoint;
}
