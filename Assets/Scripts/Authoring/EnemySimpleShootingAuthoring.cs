using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySimpleShootingAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown;

    public class Baker : Baker<EnemySimpleShootingAuthoring>
    {
        public override void Bake(EnemySimpleShootingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemySimpleShooting
            {
                bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                cooldown = authoring.cooldown,
                currentCooldown = 0
            });
        }
    }
}

public partial struct EnemySimpleShooting : IComponentData
{
    public Entity bulletPrefab;
    public float cooldown;
    public float currentCooldown;
}
