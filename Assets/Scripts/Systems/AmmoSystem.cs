using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct AmmoSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<PlayerShooting> ammo1 in SystemAPI.Query<RefRW<PlayerShooting>>())
        {
            if (ammo1.ValueRO.currentAmo <= 0)
            {
                ammo1.ValueRW.currentReload += SystemAPI.Time.DeltaTime;
                if (ammo1.ValueRO.currentReload >= ammo1.ValueRO.maxReloadSpeed)
                {
                    ammo1.ValueRW.currentAmo = ammo1.ValueRO.maxAmo + ammo1.ValueRO.extraAmo;
                    ammo1.ValueRW.currentReload = 0;
                }
            }
        }

        foreach (RefRW<PlayerShootingV2> ammo2 in SystemAPI.Query<RefRW<PlayerShootingV2>>())
        {
            if (ammo2.ValueRO.currentAmo <= 0)
            {
                ammo2.ValueRW.currentReload += SystemAPI.Time.DeltaTime;
                if (ammo2.ValueRO.currentReload >= ammo2.ValueRO.maxReloadSpeed)
                {
                    ammo2.ValueRW.currentAmo = ammo2.ValueRO.maxAmo + ammo2.ValueRO.extraAmo;
                    ammo2.ValueRW.currentReload = 0;
                }
            }
        }
    }
}
