
using Unity.Entities;
using UnityEngine;

public class PlayerControllingAuthoring : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public class Baker : Baker<PlayerControllingAuthoring>
    {
        public override void Bake(PlayerControllingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerControl {
                maxSpeed = authoring.maxSpeed,
                acceleration = authoring.acceleration,
            });
        }
    }
}

public struct PlayerControl: IComponentData
{
    public float maxSpeed;
    public float acceleration;
}
