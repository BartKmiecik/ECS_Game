using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
    public int maxHealth;

    public class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Health
            {
                maxHealth = authoring.maxHealth,
                currentHealth = authoring.maxHealth,
                isAlive = true
            });
        }
    }
}

public struct Health : IComponentData
{
    public int maxHealth;
    public int currentHealth;
    public bool isAlive;
}