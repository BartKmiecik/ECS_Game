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
    public bool automatic;
    EntityManager manager;
    private float _cd;
    private int _damage;
    private bool _weaponEnabled;

    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _cd = 0;
        _damage = 0;
        automatic = false;
        foreach (RefRO<PlayerShootingV2> player in SystemAPI.Query<RefRO<PlayerShootingV2>>())
        {
            _weaponEnabled = player.ValueRO.enabled;
            break;
        }
    }

    public void UpdateCoolDown(float cooldown)
    {
        if (_cd == 0)
        {
            _cd = cooldown;
        }
    }

    public void UpdateDamage(int damage)
    {
        _damage = damage;
    }

    public void EnableWeapon(bool weaponEnabled)
    {
        _weaponEnabled = weaponEnabled;
    }


    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        foreach ((RefRW<PlayerShootingV2> player, RefRO<LocalToWorld> localTransform, RefRW<PhysicsVelocity> physicsVelocity) 
            in SystemAPI.Query<RefRW<PlayerShootingV2>, RefRO<LocalToWorld>, RefRW<PhysicsVelocity>> ())
        {
            if (player.ValueRO.enabled != _weaponEnabled) { player.ValueRW.enabled = _weaponEnabled; }
            if (!player.ValueRO.enabled) { return; }
            if (_cd > 0)
            {
                player.ValueRW.extraCd += _cd;
                _cd = 0;
            }
            var fireMB = Input.GetMouseButton(1);
            if (player.ValueRO.currentAmo > 0)
            {
                if (fireMB || automatic)
                {
                    if (player.ValueRO.currentCooldown >= Mathf.Max(player.ValueRO.cooldown - player.ValueRO.extraCd, 0))
                    {
                        if (_damage == 0)
                        {
                            _damage = player.ValueRO.damage;
                        }
                        float _force = player.ValueRO.force;
                        Entity _bulletPrefab = player.ValueRO.bulletPrefab;
                        var temp = localTransform.ValueRO.Value;

                        if (automatic)
                        {
                            float3 closestTarget = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AutomaticTargetHolder>().GetClosestTarget();
                            if (math.all(closestTarget == float3.zero))
                                return;
                            player.ValueRW.currentAmo -= 1;
                            Entity spawnedEntity = manager.Instantiate(_bulletPrefab);
                            Quaternion rot = Quaternion.LookRotation(math.normalize(closestTarget - localTransform.ValueRO.Position), math.up());

                            LocalTransform tmp = new LocalTransform
                            {
                                Position = temp.Translation(),
                                Rotation = rot,
                                Scale = 1
                            };

                            manager.SetComponentData(spawnedEntity, tmp);
                            manager.SetComponentData<Bullet>(spawnedEntity, new Bullet
                            {
                                damage_value = _damage,
                                speed = manager.GetComponentData<Bullet>(spawnedEntity).speed,
                                force = _force
                            });

                            float3 temp2 = new float3(tmp.Forward());
                            physicsVelocity.ValueRW.Linear -= temp2 * _force;
                        }
                        else
                        {
                            player.ValueRW.currentAmo -= 1;
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
                                speed = manager.GetComponentData<Bullet>(spawnedEntity).speed,
                                force = _force
                            });
                            float3 temp2 = new float3(localTransform.ValueRO.Forward);
                            physicsVelocity.ValueRW.Linear -= temp2 * _force;
                        }

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
}
