using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct PlayerShootingSystemV2 : ISystem
{
    public bool paused;
    EntityManager manager;
    private float _cd;
    private int _damage;

    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _cd = 0;
        _damage = 0;
    }

    public void UpdateCoolDown(float cooldown)
    {
        _cd = cooldown;
    }

    public void UpdateDamage(int damage)
    {
        _damage = damage;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        foreach ((RefRW<PlayerShootingV2> player, RefRO<LocalToWorld> localTransform, RefRW<PhysicsVelocity> physicsVelocity) 
            in SystemAPI.Query<RefRW<PlayerShootingV2>, RefRO<LocalToWorld>, RefRW<PhysicsVelocity>> ())
        {
            if (_cd > 0)
            {
                player.ValueRW.cooldown -= _cd;
                _cd = 0;
            }
            var fireMB = Input.GetMouseButton(1);
            if (fireMB)
            {
                if (player.ValueRO.currentCooldown >= Mathf.Max(player.ValueRO.cooldown, 0))
                {
                    if (_damage == 0)
                    {
                        _damage = player.ValueRO.damage;
                    }
                    float _force = player.ValueRO.force;
                    Entity _bulletPrefab = player.ValueRO.bulletPrefab;
                    var temp = localTransform.ValueRO.Value;
                    Entity spawnedEntity = manager.Instantiate(_bulletPrefab);
                    manager.SetComponentData(spawnedEntity, new LocalTransform
                    {
                        Position = temp.Translation(),
                        Rotation = temp.Rotation(),
                        Scale = 1
                    });
                    manager.SetComponentData<Bullet>(spawnedEntity, new Bullet
                    {
                        damage_value = _damage,
                        speed = manager.GetComponentData<Bullet>(spawnedEntity).speed
                    });
                    float3 temp2 = new float3(localTransform.ValueRO.Forward);
                    physicsVelocity.ValueRW.Linear -= temp2 * _force;
                    player.ValueRW.currentCooldown = 0;
                }
            }
            if (player.ValueRO.currentCooldown < Mathf.Max(player.ValueRO.cooldown, 0))
            {
                player.ValueRW.currentCooldown += SystemAPI.Time.DeltaTime;
            }
        }
    }
}
