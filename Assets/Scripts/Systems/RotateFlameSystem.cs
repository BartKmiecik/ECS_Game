using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct RotateFlameSystem : ISystem
{
    public float cooldown;
    public float currentCooldown;
    public float duration;
    public float currentDuration;
    public float rotationSpeed;
    public float3 playerPos;

    EntityManager manager;

    public void OnCreate(ref SystemState state)
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void OnUpdate(ref SystemState state) 
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach ((RefRW<RotatingFlame> rotateFlame, RefRO<LocalToWorld> localTransform) in SystemAPI.Query<RefRW<RotatingFlame>, RefRO<LocalToWorld>>())
        {
            cooldown = rotateFlame.ValueRO.cooldown;
            currentCooldown = rotateFlame.ValueRO.currentCooldown;
            duration = rotateFlame.ValueRO.duration;
            currentDuration = rotateFlame.ValueRO.currentDuration;
            rotationSpeed = rotateFlame.ValueRO.rotationSpeed;
            playerPos = localTransform.ValueRO.Position;

            if ((currentDuration >= duration) && (currentCooldown <= 0))
            {
                rotateFlame.ValueRW.currentDuration -= deltaTime;
                rotateFlame.ValueRW.currentCooldown = cooldown;
                Entity prefab = rotateFlame.ValueRO.beamPrefab;
                var temp = localTransform.ValueRO.Value;
                Entity spawnedBeam = manager.Instantiate(prefab);
                manager.SetComponentData(spawnedBeam, new LocalTransform
                {
                    Position = temp.Translation(),
                    Rotation = temp.Rotation(),
                    Scale = 1
                });
                manager.SetComponentData(spawnedBeam, new Lifespan
                {
                    maxLife = duration,
                    currentLife = duration,
                });
            }

            if (currentDuration > 0 && currentDuration < duration)
            {
                rotateFlame.ValueRW.currentDuration -= deltaTime;
            }else if (currentCooldown > 0 && (currentDuration <= 0 || currentDuration == duration))
            {
                rotateFlame.ValueRW.currentDuration = duration;
                rotateFlame.ValueRW.currentCooldown -= deltaTime;
            }
        }
        

        if (currentCooldown > 0 && currentDuration != duration)
        {
            currentDuration -= deltaTime;
        }

        RotateBeamJob rotateBeamJob = new RotateBeamJob 
        {
            deltaTime = deltaTime,
            rotateSpeed = rotationSpeed,
            playerPos = playerPos,
        };
        rotateBeamJob.Schedule();
    }

    public partial struct RotateBeamJob : IJobEntity
    {
        public float deltaTime;
        public float rotateSpeed;
        public float3 playerPos;
        public void Execute(in FireBeam _, ref LocalTransform localTransform)
        {
            localTransform.Position = playerPos;
            localTransform = localTransform.RotateY(rotateSpeed * deltaTime);
        }
    }
}
