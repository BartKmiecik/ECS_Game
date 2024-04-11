using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DamagableByAreaAuthoring : MonoBehaviour
{
    public class Baker : Baker<DamagableByAreaAuthoring>
    {
        public override void Bake(DamagableByAreaAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new DamagableByArea
            {
                currentCooldown = 0,
            }) ;
        }
    }
}

public partial struct DamagableByArea : IComponentData
{
    public float currentCooldown;
}
