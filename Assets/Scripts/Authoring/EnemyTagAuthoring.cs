using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemyTagAuthoring : MonoBehaviour
{
    public int attackValue;
    public class Baker : Baker<EnemyTagAuthoring>
    {
        public override void Bake(EnemyTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyTag { 
                attackValue = authoring.attackValue,
            });
        }
    }
}

public struct EnemyTag : IComponentData
{
    public int attackValue;
}
