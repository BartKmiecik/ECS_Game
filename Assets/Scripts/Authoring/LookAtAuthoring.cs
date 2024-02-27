using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class LookAtAuthoring : MonoBehaviour
{
    public Transform target;

    private class Baker : Baker<LookAtAuthoring>
    {
        public override void Bake(LookAtAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new LookAt
            {
                target = authoring.target.position
            });
        }
    }
}

public struct LookAt : IComponentData
{
    public float3 target;
}