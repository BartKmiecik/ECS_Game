using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ExpirienceAuthoring : MonoBehaviour
{
    public int expirience;

    public class Baker : Baker<ExpirienceAuthoring>
    {
        public override void Bake(ExpirienceAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Expirience
            {
                exprience = authoring.expirience,
            });
        }
    }
}

public partial struct Expirience : IComponentData
{
    public int exprience;
}
