using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PlayerGameObjectPrefab : IComponentData
{
    public GameObject Value;
}

public class PlayerAnimatorReference : ICleanupComponentData
{
    public Animator Value;
}

public class AnimatorAuthoring : MonoBehaviour
{
    public GameObject PlayerGameObjectPrefab;

    public class PlayerGameObjectPrefabBaker : Baker<AnimatorAuthoring>
    {
        public override void Bake(AnimatorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new PlayerGameObjectPrefab { Value = authoring.PlayerGameObjectPrefab });
        }
    }
}
