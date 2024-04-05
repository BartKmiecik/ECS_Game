using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FireBeamAuthoring : MonoBehaviour
{
    public class Baker : Baker<FireBeamAuthoring>
    {
        public override void Bake(FireBeamAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FireBeam { });
        }
    }
}

public partial struct FireBeam : IComponentData
{

}
