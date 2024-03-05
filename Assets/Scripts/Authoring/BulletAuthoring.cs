using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public int damage_value;
    public float speed;
    public float lifespan;

    public class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bullet
            {
                damage_value = authoring.damage_value,
                speed = authoring.speed
            });
        }
    }
}

public partial struct Bullet : IComponentData
{
    public int damage_value;
    public float speed;
}
