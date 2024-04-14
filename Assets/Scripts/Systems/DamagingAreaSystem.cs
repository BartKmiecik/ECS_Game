using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;


[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct DamagingAreaSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DamagingArea>();
    }

    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new DamagingAreaJob
        {
            damagingArea = SystemAPI.GetComponentLookup<DamagingArea>(),
            damagable = SystemAPI.GetComponentLookup<DamagableByArea>(),
            enemyHealth = SystemAPI.GetComponentLookup<Health>(),
            player = SystemAPI.GetComponentLookup<Player>(),
            deltaTime = SystemAPI.Time.DeltaTime,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    public partial struct DamagingAreaJob : ITriggerEventsJob
    {
        public ComponentLookup<DamagingArea> damagingArea;
        public ComponentLookup<DamagableByArea> damagable;
        public ComponentLookup<Health> enemyHealth;
        public ComponentLookup<Player> player;
        public float deltaTime;

        public void Execute(Unity.Physics.TriggerEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isAArea = damagingArea.HasComponent(entityA);
            bool isBArea = damagingArea.HasComponent(entityB);

            if (isAArea && isBArea)
            {
                return;
            }

            bool isADamagable = damagable.HasComponent(entityA);
            bool isBDamagable = damagable.HasComponent(entityB);


            if (isADamagable && isBDamagable)
            {
                return;
            }

            if (!isADamagable && !isBDamagable)
            {
                return;
            }

            if (isADamagable && isBArea)
            {
                int damage_recived = damagingArea.GetRefRW(entityB).ValueRW.damage;
                float damage_cooldown = damagingArea.GetRefRW(entityB).ValueRW.damageCooldown;
                damagable.GetRefRW(entityA).ValueRW.currentCooldown += deltaTime;
                if (damagable.GetRefRW(entityA).ValueRO.currentCooldown >= damage_cooldown)
                {
                    damagable.GetRefRW(entityA).ValueRW.currentCooldown = 0;
                    if (enemyHealth.HasComponent(entityA))
                    {
                        enemyHealth.GetRefRW(entityA).ValueRW.currentHealth -= damage_recived;
                    }
                    if (player.HasComponent(entityA))
                    {
                        player.GetRefRW(entityA).ValueRW.currentHealth -= damage_recived;
                    }
                }
            }
            if (isBDamagable && isAArea)
            {
                int damage_recived = damagingArea.GetRefRW(entityA).ValueRW.damage;
                float damage_cooldown = damagingArea.GetRefRW(entityA).ValueRW.damageCooldown;
                damagable.GetRefRW(entityB).ValueRW.currentCooldown += deltaTime;
                if (damagable.GetRefRW(entityB).ValueRO.currentCooldown >= damage_cooldown)
                {
                    damagable.GetRefRW(entityB).ValueRW.currentCooldown = 0;
                    if (enemyHealth.HasComponent(entityB))
                    {
                        enemyHealth.GetRefRW(entityB).ValueRW.currentHealth -= damage_recived;
                    }
                    if (player.HasComponent(entityB))
                    {
                        player.GetRefRW(entityB).ValueRW.currentHealth -= damage_recived;
                    }
                }
            }
        }
    }
}
