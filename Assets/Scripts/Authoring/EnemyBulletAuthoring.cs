using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemyBulletAuthoring : MonoBehaviour
{
    public int damage_value;
    public float speed;
    public float force;

    public class Baker : Baker<EnemyBulletAuthoring>
    {
        public override void Bake(EnemyBulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyBullet
            {
                damage_value = authoring.damage_value,
                speed = authoring.speed,
                force = authoring.force,
            });
        }
    }
}

public partial struct EnemyBullet : IComponentData
{
    public int damage_value;
    public float speed;
    public float force;
}