using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DestrutableAuthoring : MonoBehaviour
{
    private bool shouldBeDestroyed = false;

    public class Baker : Baker<DestrutableAuthoring>
    {
        public override void Bake(DestrutableAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Destructable {
            shouldBeDestroyed = authoring.shouldBeDestroyed
            });
        }
    }
}

public partial struct Destructable : IComponentData
{
    public bool shouldBeDestroyed;
}
