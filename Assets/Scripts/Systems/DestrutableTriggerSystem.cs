using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct DestrutableTriggerSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new CountNumTriggerEvents
        {
            destructable = SystemAPI.GetComponentLookup<Destructable>(),
            bullet = SystemAPI.GetComponentLookup<Bullet>(),
            health = SystemAPI.GetComponentLookup<Health>(),
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

    }

    public partial struct CountNumTriggerEvents : ITriggerEventsJob
    {
        public ComponentLookup<Destructable> destructable;
        public ComponentLookup<Bullet> bullet;
        public ComponentLookup<Health> health;

        public void Execute(Unity.Physics.TriggerEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isBodyATrigger = destructable.HasComponent(entityA);
            bool isBodyBTrigger = destructable.HasComponent(entityB);

            if (!isBodyATrigger || !isBodyBTrigger)
            {
                return;
            }    

            bool isABullet = bullet.HasComponent(entityA);
            bool isBBullet = bullet.HasComponent(entityB);

            if (isABullet && isBBullet)
            {
                return;
            }

            bool hasAHealth = health.HasComponent(entityA);
            bool hasBHealth = health.HasComponent(entityB);

            if (hasAHealth)
            {
                int damage_recived = bullet.GetRefRW(entityB).ValueRW.damage_value;
                health.GetRefRW(entityA).ValueRW.currentHealth -= damage_recived;
            }
            else
                destructable.GetRefRW(entityA).ValueRW.shouldBeDestroyed = true;
            if (hasBHealth)
            {
                int damage_recived = bullet.GetRefRW(entityA).ValueRW.damage_value;
                health.GetRefRW(entityB).ValueRW.currentHealth -= damage_recived;
            }
            else
                destructable.GetRefRW(entityB).ValueRW.shouldBeDestroyed = true;
        }
    }
}

