using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FollowFlowFieldAuthoring : MonoBehaviour
{
    public class Baker : Baker<FollowFlowFieldAuthoring>
    {
        public override void Bake(FollowFlowFieldAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FollowFlowField { });
        }
    }
}

public partial struct FollowFlowField : IComponentData
{

}
