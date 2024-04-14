using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ExpirienceAuthoring : MonoBehaviour
{
    public int expirience;
    public float collectingFlySpeed;

    public class Baker : Baker<ExpirienceAuthoring>
    {
        public override void Bake(ExpirienceAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Expirience
            {
                exprience = authoring.expirience,
                shouldBeCollected = false,
                collectingFlySpeed = authoring.collectingFlySpeed,
            });
        }
    }
}

public partial struct Expirience : IComponentData
{
    public int exprience;
    public bool shouldBeCollected;
    public float collectingFlySpeed;
}
