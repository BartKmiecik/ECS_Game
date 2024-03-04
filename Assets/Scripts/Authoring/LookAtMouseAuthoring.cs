using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class LookAtMouseAuthoring : MonoBehaviour
{
    public class Baker : Baker<LookAtMouseAuthoring>
    {
        public override void Bake(LookAtMouseAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new LookAtMouse { });
        }
    }
}

public partial struct LookAtMouse : IComponentData
{

}
