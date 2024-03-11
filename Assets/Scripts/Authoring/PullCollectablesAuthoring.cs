using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PullCollectablesAuthoring : MonoBehaviour
{
    public float distance;

    public class Baker : Baker<PullCollectablesAuthoring>
    {
        public override void Bake(PullCollectablesAuthoring authoring)
        {
            Entity enity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(enity, new PullCollectables
            {
                distance = authoring.distance
            });
        }
    }

}

public partial struct PullCollectables : IComponentData
{
    public float distance;
}
