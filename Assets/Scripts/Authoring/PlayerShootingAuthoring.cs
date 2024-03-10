using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerShootingAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown;

    public class Baker : Baker<PlayerShootingAuthoring>
    {
        public override void Bake(PlayerShootingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerShooting
            {
                buletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                cooldown = authoring.cooldown,
                currentCooldown = authoring.cooldown
            });
        }
    }
}

public partial struct PlayerShooting : IComponentData
{
    public Entity buletPrefab;
    public float cooldown;
    public float currentCooldown;
}
