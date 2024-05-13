using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
    public int maxHealth;
    public GameObject healthChangePopup;

    public class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Health
            {
                maxHealth = authoring.maxHealth,
                currentHealth = authoring.maxHealth,
                isAlive = true,
                healthChangePopup = GetEntity(authoring.healthChangePopup, TransformUsageFlags.Dynamic),
                showPopup = false,
                lastRecievedHit = 0,
            });
        }
    }
}

public struct Health : IComponentData
{
    public int maxHealth;
    public int currentHealth;
    public bool isAlive;
    public Entity healthChangePopup;
    public bool showPopup;
    public int lastRecievedHit;
}