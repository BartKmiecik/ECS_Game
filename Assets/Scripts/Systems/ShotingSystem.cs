using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct ShotingSystem : ISystem
{
    public bool paused;
    public bool automatic;
    EntityManager manager;
    private float _cd;
    private int _extraDamage;

    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _cd = 0;
        _extraDamage = 0;
        automatic = true;
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
        _extraDamage += damage;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        foreach ((RefRW<PlayerShooting> player, RefRO<LocalToWorld> localTransform) 
            in SystemAPI.Query<RefRW<PlayerShooting>, RefRO<LocalToWorld>>())
        {
            if (_cd > 0)
            {
                player.ValueRW.extraCd += _cd;
                _cd = 0;
            }
            var fireMB = Input.GetMouseButton(0);
            if (player.ValueRO.currentAmo > 0) 
            {
                if (fireMB || automatic)
                {
                    if (player.ValueRO.currentCooldown >= Mathf.Max(player.ValueRO.cooldown - player.ValueRO.extraCd, 0))
                    {
                        if (automatic)
                        {
                            float3 closestTarget = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AutomaticTargetHolder>().GetClosestTarget();
                            if (math.all(closestTarget == float3.zero))
                                return;
                            player.ValueRW.currentAmo -= 1;
                            Entity prefab = player.ValueRO.buletPrefab;
                            var temp = localTransform.ValueRO.Value;
                            Entity spawnedEntity = manager.Instantiate(prefab);
                            closestTarget.y = localTransform.ValueRO.Position.y;
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
                                damage_value = manager.GetComponentData<Bullet>(spawnedEntity).damage_value + _extraDamage,
                                speed = manager.GetComponentData<Bullet>(spawnedEntity).speed
                            });
                        }
                        else
                        {
                            player.ValueRW.currentAmo -= 1;
                            Entity prefab = player.ValueRO.buletPrefab;
                            var temp = localTransform.ValueRO.Value;
                            Entity spawnedEntity = manager.Instantiate(prefab);
                            manager.SetComponentData(spawnedEntity, new LocalTransform
                            {
                                Position = temp.Translation(),
                                Rotation = temp.Rotation(),
                                Scale = 1
                            });
                            manager.SetComponentData<Bullet>(spawnedEntity, new Bullet
                            {
                                damage_value = manager.GetComponentData<Bullet>(spawnedEntity).damage_value + _extraDamage,
                                speed = manager.GetComponentData<Bullet>(spawnedEntity).speed
                            });
                        }
                        player.ValueRW.currentCooldown = 0;
                    }
                }
                if (player.ValueRO.currentCooldown < Mathf.Max(player.ValueRO.cooldown - player.ValueRO.extraCd, 0))
                {
                    player.ValueRW.currentCooldown += SystemAPI.Time.DeltaTime;
                }
            }
        }
    }
}
