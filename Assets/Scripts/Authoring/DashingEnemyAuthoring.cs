using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DashingEnemyAuthoring : MonoBehaviour
{
    public float force;
    public float cooldown;

    public class Baker : Baker<DashingEnemyAuthoring>
    {
        public override void Bake(DashingEnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new DashingEnemy
            {
                force = authoring.force,
                cooldown = authoring.cooldown,
                currentCooldown = authoring.cooldown
            });
        }
    }
}

public partial struct DashingEnemy : IComponentData
{
    public float force;
    public float cooldown;
    public float currentCooldown;
}
