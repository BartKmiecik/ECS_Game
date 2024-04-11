using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DamagingAreaAuthoring : MonoBehaviour
{
    public int damage;
    public int damageCooldown;

    public class Baker : Baker<DamagingAreaAuthoring>
    {
        public override void Bake(DamagingAreaAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new DamagingArea
            {
                damage = authoring.damage,
                damageCooldown = authoring.damageCooldown,
            });
        }
    }
}

public partial struct DamagingArea : IComponentData
{
    public int damage;
    public float damageCooldown;
}
