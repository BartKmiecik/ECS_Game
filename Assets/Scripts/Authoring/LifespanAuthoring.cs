using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class LifespanAuthoring : MonoBehaviour
{
    public float lifespan;

    public class Baker : Baker<LifespanAuthoring>
    {
        public override void Bake(LifespanAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Lifespan
            {
                maxLife = authoring.lifespan,
                currentLife = authoring.lifespan
            });
        }
    }
}

public partial struct Lifespan : IComponentData
{
    public float maxLife;
    public float currentLife;
}