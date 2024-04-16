using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct AmmoSystem : ISystem
{
    private int w1ExtraAmmo;
    private int w2ExtraAmmo;
    private float w1ReloadReduction;
    private float w2ReloadReduction;

    public void Weapon1IncreaseAmmo(int extraAmmo)
    {
        if (w1ExtraAmmo == 0)
        {
            w1ExtraAmmo = extraAmmo;
        }
    }

    public void Weapon2IncreaseAmmo(int extraAmmo)
    {
        if (w2ExtraAmmo == 0)
        {
            w2ExtraAmmo = extraAmmo;
        }
    }

    public void Weapon1ReloadReuction(float reduction)
    {
        if (w1ReloadReduction == 0)
        {
            w1ReloadReduction = reduction;
        }
    }

    public void Weapon2ReloadReuction(float reduction)
    {
        if (w2ReloadReduction == 0)
        {
            w2ReloadReduction = reduction;
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<PlayerShooting> ammo1 in SystemAPI.Query<RefRW<PlayerShooting>>())
        {
            if (w1ExtraAmmo > 0)
            {
                ammo1.ValueRW.extraAmo += w1ExtraAmmo;
                w1ExtraAmmo = 0;
            }

            if (w1ReloadReduction > 0)
            {
                ammo1.ValueRW.reloadSpeedReduction += w1ReloadReduction;
                w1ReloadReduction = 0;
            }

            if (ammo1.ValueRO.currentAmo <= 0)
            {
                ammo1.ValueRW.currentReload += SystemAPI.Time.DeltaTime;
                if (ammo1.ValueRO.currentReload >= ammo1.ValueRO.maxReloadSpeed - ammo1.ValueRO.reloadSpeedReduction)
                {
                    ammo1.ValueRW.currentAmo = ammo1.ValueRO.maxAmo + ammo1.ValueRO.extraAmo;
                    ammo1.ValueRW.currentReload = 0;
                }
            }
        }

        foreach (RefRW<PlayerShootingV2> ammo2 in SystemAPI.Query<RefRW<PlayerShootingV2>>())
        {
            if (w2ExtraAmmo > 0)
            {
                ammo2.ValueRW.extraAmo += w2ExtraAmmo;
                w2ExtraAmmo = 0;
            }

            if (w2ReloadReduction > 0)
            {
                ammo2.ValueRW.reloadSpeedReduction += w2ReloadReduction;
                w2ReloadReduction = 0;
            }

            if (ammo2.ValueRO.currentAmo <= 0)
            {
                ammo2.ValueRW.currentReload += SystemAPI.Time.DeltaTime;
                if (ammo2.ValueRO.currentReload >= ammo2.ValueRO.maxReloadSpeed - ammo2.ValueRO.reloadSpeedReduction)
                {
                    ammo2.ValueRW.currentAmo = ammo2.ValueRO.maxAmo + ammo2.ValueRO.extraAmo;
                    ammo2.ValueRW.currentReload = 0;
                }
            }
        }
    }
}
