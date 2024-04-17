using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerShootingV2Authoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float force;
    public int damage;
    public float cooldown;
    public int maxAmo;
    public float maxReloadSpeed;
    public bool enabledByDefault;

    public class Baker : Baker<PlayerShootingV2Authoring>
    {
        public override void Bake(PlayerShootingV2Authoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerShootingV2
            {
                bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                force = authoring.force,
                damage = authoring.damage,
                cooldown = authoring.cooldown,
                currentCooldown = authoring.cooldown,
                extraCd = 0,
                maxAmo = authoring.maxAmo,
                currentAmo = authoring.maxAmo,
                maxReloadSpeed = authoring.maxReloadSpeed,
                currentReload = 0,
                extraAmo = 0,
                reloadSpeedReduction = 0,
                enabled = authoring.enabledByDefault,
            });
        }
    }

}

public partial struct PlayerShootingV2 : IComponentData
{
    public Entity bulletPrefab;
    public float force;
    public int damage;
    public float cooldown;
    public float currentCooldown;
    public float extraCd;
    public int maxAmo;
    public int extraAmo;
    public int currentAmo;
    public float maxReloadSpeed;
    public float currentReload;
    public float reloadSpeedReduction;
    public bool enabled;
}
