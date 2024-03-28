using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct ShotingSystem : ISystem
{
    public bool paused;
    EntityManager manager;
    private float _cd;
    private int _extraDamage;
    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _cd = 0;
        _extraDamage = 0;
    }

    public void UpdateCoolDown(float cooldown)
    {
        _cd = cooldown;
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
            if (fireMB)
            {
                if (player.ValueRO.currentCooldown >= Mathf.Max(player.ValueRO.cooldown - player.ValueRO.extraCd, 0))
                {
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
