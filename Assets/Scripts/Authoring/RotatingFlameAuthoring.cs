using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RotatingFlameAuthoring : MonoBehaviour
{
    public GameObject beamPrefab;
    public int beams;
    public float distance;
    public float cooldown;
    public float duration;
    public float rotationSpeed;
    public int damage;

    public class Baker : Baker<RotatingFlameAuthoring>
    {
        public override void Bake(RotatingFlameAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotatingFlame
            {
                beamPrefab = GetEntity(authoring.beamPrefab, TransformUsageFlags.Dynamic),
                beams = authoring.beams,
                distance = authoring.distance,
                cooldown = authoring.cooldown,
                duration = authoring.duration,
                rotationSpeed = authoring.rotationSpeed,
                damage = authoring.damage,
                currentCooldown = 0,
                currentDuration = authoring.duration,
                extraDamage = 0,
            });
        }
    }
}

public partial struct RotatingFlame : IComponentData
{
    public Entity beamPrefab;
    public int beams;
    public float distance;
    public float cooldown;
    public float currentCooldown;
    public float duration;
    public float currentDuration;
    public float rotationSpeed;
    public int damage;
    public int extraDamage;
}
