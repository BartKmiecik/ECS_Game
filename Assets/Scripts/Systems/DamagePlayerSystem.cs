using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct DamagePlayerSystem : ISystem
{
    private float playerInvincibilityTime;
    public bool paused;
    public void OnCreate(ref SystemState state)
    {
        foreach (RefRO<Player> player in SystemAPI.Query<RefRO<Player>>())
        {
            playerInvincibilityTime = player.ValueRO.playerInvincibilityTime;
            break;
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        foreach (RefRW<Player> player in SystemAPI.Query<RefRW<Player>>())
        {
            if (player.ValueRO.timer > 0f)
            {
                player.ValueRW.timer -= Time.deltaTime;
                return;
            }
            break;
        }

        state.Dependency = new EnemyCollideWithPlayer
        {
            player = SystemAPI.GetComponentLookup<Player>(),
            enemy = SystemAPI.GetComponentLookup<EnemyTag>(),
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    public partial struct EnemyCollideWithPlayer : ICollisionEventsJob
    {
        public ComponentLookup<Player> player;
        public ComponentLookup<EnemyTag> enemy;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isAPlayer = player.HasComponent(entityA);
            bool isBPlayer = player.HasComponent(entityB);


            if (isAPlayer && isBPlayer)
            {
                return;
            }

            bool isAEnemy = enemy.HasComponent(entityA);
            bool isBEnemy = enemy.HasComponent(entityB);

            if (isAEnemy && isBEnemy)
            {
                return;
            }

            if (!isAEnemy && !isBEnemy)
            {
                return;
            }


            if (isAPlayer && isBEnemy)
            {
                if (player.GetRefRO(entityA).ValueRO.timer > 0)
                    return;
                player.GetRefRW(entityA).ValueRW.timer = player.GetRefRO(entityA).ValueRO.playerInvincibilityTime;
                int damage_recived = enemy.GetRefRW(entityB).ValueRW.attackValue;
                player.GetRefRW(entityA).ValueRW.currentHealth -= damage_recived;
            }
            if (isBPlayer && isAEnemy)
            {
                if (player.GetRefRO(entityB).ValueRO.timer > 0)
                    return;
                player.GetRefRW(entityB).ValueRW.timer = player.GetRefRO(entityB).ValueRO.playerInvincibilityTime;
                int damage_recived = enemy.GetRefRW(entityA).ValueRW.attackValue;
                player.GetRefRW(entityB).ValueRW.currentHealth -= damage_recived;
            }
        }
    }
}
