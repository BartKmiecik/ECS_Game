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
public partial struct EBulletsTriggerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new EnemyBulletsBulletsCollideEventsJob
        {
            destructable = SystemAPI.GetComponentLookup<Destructable>(),
            bullet = SystemAPI.GetComponentLookup<EnemyBullet>(),
            health = SystemAPI.GetComponentLookup<Player>(),
            localTransform = SystemAPI.GetComponentLookup<LocalTransform>(),
            physicsVelocity = SystemAPI.GetComponentLookup<PhysicsVelocity>(),
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

    }

    public partial struct EnemyBulletsBulletsCollideEventsJob : ITriggerEventsJob
    {
        public ComponentLookup<Destructable> destructable;
        public ComponentLookup<EnemyBullet> bullet;
        public ComponentLookup<Player> health;
        public ComponentLookup<LocalTransform> localTransform;
        public ComponentLookup<PhysicsVelocity> physicsVelocity;

        public void Execute(Unity.Physics.TriggerEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isABullet = bullet.HasComponent(entityA);
            bool isBBullet = bullet.HasComponent(entityB);


            if (isABullet && isBBullet)
            {
                return;
            }

            bool hasAHealth = health.HasComponent(entityA);
            bool hasBHealth = health.HasComponent(entityB);

            if (hasAHealth && hasBHealth)
            {
                return;
            }

            if (!hasAHealth && !hasBHealth)
            {
                return;
            }

            if (hasAHealth && isBBullet)
            {
                if (health.GetRefRO(entityA).ValueRO.timer > 0)
                    return;
                health.GetRefRW(entityA).ValueRW.timer = health.GetRefRO(entityA).ValueRO.playerInvincibilityTime;
                int damage_recived = bullet.GetRefRW(entityB).ValueRW.damage_value;
                health.GetRefRW(entityA).ValueRW.currentHealth -= damage_recived;
                health.GetRefRW(entityA).ValueRW.lastRecievedHit = damage_recived;
                health.GetRefRW(entityA).ValueRW.showPopup = true;
                if (destructable.HasComponent(entityB))
                {
                    destructable.GetRefRW(entityB).ValueRW.shouldBeDestroyed = true;
                }
                if (bullet.GetRefRO(entityB).ValueRO.force > 0)
                {
                    float force = bullet.GetRefRO(entityB).ValueRO.force / 10;
                    float3 temp2 = new float3(localTransform.GetRefRW(entityA).ValueRW.Forward());
                    physicsVelocity.GetRefRW(entityA).ValueRW.Linear -= temp2 * force;
                }
            }
            if (hasBHealth && isABullet)
            {
                if (health.GetRefRO(entityB).ValueRO.timer > 0)
                    return;
                health.GetRefRW(entityB).ValueRW.timer = health.GetRefRO(entityA).ValueRO.playerInvincibilityTime;
                int damage_recived = bullet.GetRefRW(entityA).ValueRW.damage_value;
                health.GetRefRW(entityB).ValueRW.currentHealth -= damage_recived;
                health.GetRefRW(entityB).ValueRW.lastRecievedHit = damage_recived;
                health.GetRefRW(entityB).ValueRW.showPopup = true;
                if (destructable.HasComponent(entityA))
                {
                    destructable.GetRefRW(entityA).ValueRW.shouldBeDestroyed = true;
                }
                if (bullet.GetRefRO(entityA).ValueRO.force > 0)
                {
                    float force = bullet.GetRefRO(entityA).ValueRO.force / 10;
                    float3 temp2 = new float3(localTransform.GetRefRW(entityB).ValueRW.Forward());
                    physicsVelocity.GetRefRW(entityB).ValueRW.Linear -= temp2 * force;
                }
            }
        }
    }
}


