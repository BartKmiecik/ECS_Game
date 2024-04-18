using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SinusAnimationAuthoring : MonoBehaviour
{
    public Vector3 localPos;
    public float speed;
    public float magnitude;

    private void Start()
    {
        localPos = transform.localPosition;
        Debug.Log(localPos);
    }

    public class Baker : Baker<SinusAnimationAuthoring>
    {
        public override void Bake(SinusAnimationAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SinusAnimation {
                localPos = authoring.localPos,
                speed = authoring.speed,
                magnitude = authoring.magnitude,
            });
        }
    }
}

public partial struct SinusAnimation : IComponentData
{
    public float3 localPos;
    public float speed;
    public float magnitude;
}
