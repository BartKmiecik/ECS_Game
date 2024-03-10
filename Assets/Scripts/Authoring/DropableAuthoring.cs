using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DropableAuthoring : MonoBehaviour
{
    public GameObject objectToDrop;

    public class Baker : Baker<DropableAuthoring>
    {
        public override void Bake(DropableAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Dropable
            {
                entityToDrop = GetEntity(authoring.objectToDrop, TransformUsageFlags.Dynamic)
            });
        }
    }
}

public partial struct Dropable : IComponentData
{
    public Entity entityToDrop;
}
