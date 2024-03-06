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
            destructable = SystemAPI.GetComponentLookup<Destructable>()
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

    }

    public partial struct CountNumTriggerEvents : ITriggerEventsJob
    {
        public ComponentLookup<Destructable> destructable;

        public void Execute(Unity.Physics.TriggerEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isBodyATrigger = destructable.HasComponent(entityA);
            bool isBodyBTrigger = destructable.HasComponent(entityB);

            if (isBodyATrigger && isBodyBTrigger)
                return;

            Debug.Log($"Before: {destructable.GetRefRW(entityA).ValueRW.shouldBeDestroyed}");
            destructable.GetRefRW(entityA).ValueRW.shouldBeDestroyed = true;
            Debug.Log($"After: {destructable.GetRefRW(entityA).ValueRW.shouldBeDestroyed}");
            destructable.GetRefRW(entityB).ValueRW.shouldBeDestroyed = true;
        }
    }
}

