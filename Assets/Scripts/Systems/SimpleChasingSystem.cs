using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static RotateCubeSystem;

[BurstCompile]
public partial struct SimpleChasingSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MovementSpeed>();
    }

    void OnUpdate(ref SystemState state)
    {
        SimpleChaseJob simpleChaseJob = new SimpleChaseJob
        {
            deltaTime = Time.deltaTime
        };
        simpleChaseJob.ScheduleParallel();
    }

    [WithNone(typeof(Player))]
    //[WithAll(typeof(LookAt), typeof(MovementSpeed))]
    public partial struct SimpleChaseJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref LocalTransform localTransform, in LookAt lookAt, in MovementSpeed movementSpeed)
        {
            float3 targetPos;
            targetPos.x = lookAt.target.x;
            targetPos.y = lookAt.target.y;
            targetPos.z = lookAt.target.z;


            float3 curentPos;

            curentPos.x = localTransform.Position.x;
            curentPos.y = localTransform.Position.y;
            curentPos.z = localTransform.Position.z;

            float3 relativePos = targetPos - curentPos;


            localTransform.Position = curentPos + (relativePos * movementSpeed.speed * deltaTime);
        }
    }
}
