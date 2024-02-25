using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using UnityEngine;

public class MovementSpeedAuthoring : MonoBehaviour
{
    public float speed;
    public class Baker : Baker<MovementSpeedAuthoring>
    {
        public override void Bake(MovementSpeedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MovementSpeed
            {
                speed = authoring.speed
            });
        }
    }
}

public struct MovementSpeed: IComponentData
{
    public float speed;
}
