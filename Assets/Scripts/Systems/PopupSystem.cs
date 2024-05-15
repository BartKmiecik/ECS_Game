using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct PopupSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<Health> health, RefRO<LocalTransform> localTransform) in SystemAPI.Query<RefRW<Health>, RefRO<LocalTransform>>())
        {
            if (health.ValueRO.showPopup)
            {
                health.ValueRW.showPopup = false;
                SpawnPopupDamage.INSTANCE.CreatePopUp(health.ValueRO.lastRecievedHit, localTransform.ValueRO.Position);
            }
        }
        foreach ((RefRW<Player> player, RefRO<LocalTransform> localTransform) in SystemAPI.Query<RefRW<Player>, RefRO<LocalTransform>>())
        {
            if (player.ValueRO.showPopup)
            {
                player.ValueRW.showPopup = false;
                SpawnPopupDamage.INSTANCE.CreatePlayerPopUp(player.ValueRO.lastRecievedHit, localTransform.ValueRO.Position);
                Debug.Log("Player take damage");
            }
        }
    }
}
