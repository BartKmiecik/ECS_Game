using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ColliderHolderAuthoring : MonoBehaviour
{
    public GameObject collisionPrefabDestructalbe;
    public GameObject collisionPrefabEdgeMap;

    public class Baker : Baker<ColliderHolderAuthoring>
    {
        public override void Bake(ColliderHolderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ColliderHolder
            {
                collisionPrefabDestructalbe = GetEntity(authoring.collisionPrefabDestructalbe, TransformUsageFlags.None),
                collisionPrefabEdgeMap = GetEntity(authoring.collisionPrefabEdgeMap, TransformUsageFlags.None),
            });
        }
    }
}

public partial struct ColliderHolder : IComponentData
{
    public Entity collisionPrefabDestructalbe;
    public Entity collisionPrefabEdgeMap;
}
