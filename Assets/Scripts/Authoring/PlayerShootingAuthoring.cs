using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerShootingAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown;
    public int maxAmo;
    public float maxReloadSpeed;

    public class Baker : Baker<PlayerShootingAuthoring>
    {
        public override void Bake(PlayerShootingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerShooting
            {
                buletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                cooldown = authoring.cooldown,
                currentCooldown = authoring.cooldown,
                extraCd = 0,
                maxAmo = authoring.maxAmo,
                currentAmo = authoring.maxAmo,
                maxReloadSpeed = authoring.maxReloadSpeed,
                currentReload = 0,
                extraAmo = 0,
            });
        }
    }
}

public partial struct PlayerShooting : IComponentData
{
    public Entity buletPrefab;
    public float cooldown;
    public float currentCooldown;
    public float extraCd;
    public int maxAmo;
    public int extraAmo;
    public int currentAmo;
    public float maxReloadSpeed;
    public float currentReload;
}
