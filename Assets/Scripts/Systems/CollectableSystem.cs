using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[WithNone(typeof(Health))]
public partial struct CollectableSystem : ISystem
{
/*    EntityManager entityManager;
    EntityCommandBuffer ecb;*/

    public void OnCreate(ref SystemState state)
    {
/*        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
*/    }

    
    public void OnUpdate(ref SystemState state) {
        state.Dependency = new PlayerCollectEventsJob {
            player = SystemAPI.GetComponentLookup<Player>(),
            expirience = SystemAPI.GetComponentLookup<Expirience>(),
            destructable = SystemAPI.GetComponentLookup<Destructable>(),
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    public partial struct PlayerCollectEventsJob : ITriggerEventsJob
    {
        public ComponentLookup<Player> player;
        public ComponentLookup<Expirience> expirience;
        public ComponentLookup<Destructable> destructable;

        public void Execute(Unity.Physics.TriggerEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isAPlayer = player.HasComponent(entityA);
            bool isBPlayer = player.HasComponent(entityB);

            bool isAExpirience = expirience.HasComponent(entityA);
            bool isBExpirience = expirience.HasComponent(entityB);

            if ((isAPlayer && isBPlayer) || (isAExpirience && isBExpirience))
            {
                return;
            }


            if (isAPlayer && isBExpirience)
            {
                int exp = expirience.GetRefRO(entityB).ValueRO.exprience;
                player.GetRefRW(entityA).ValueRW.expirience += exp;
                destructable.GetRefRW(entityB).ValueRW.shouldBeDestroyed = true;
            }
                
            if (isBPlayer && isAExpirience)
            {
                int exp = expirience.GetRefRO(entityA).ValueRO.exprience;
                player.GetRefRW(entityB).ValueRW.expirience += exp;
                destructable.GetRefRW(entityA).ValueRW.shouldBeDestroyed = true;
            }      
        }
    }
}
