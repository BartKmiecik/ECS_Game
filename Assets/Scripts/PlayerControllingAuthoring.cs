
using Unity.Entities;
using UnityEngine;

public class PlayerControllingAuthoring : MonoBehaviour
{
    public float speed;
    public class Baker : Baker<PlayerControllingAuthoring>
    {
        public override void Bake(PlayerControllingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerControl {
                speed = authoring.speed
            });
        }
    }
}

public struct PlayerControl: IComponentData
{
    public float speed;
}
