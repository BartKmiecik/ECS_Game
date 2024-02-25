using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

[BurstCompile]
public partial struct LookAtSystem : ISystem
{
    float3 target;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<LookAt>();
        float3 target = new float3(0, 0, 0);
    }

    void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<LocalTransform> localTransform, RefRO<Player> rotateSpeed) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Player>>())
        {
            target = localTransform.ValueRO.Position;
        }

        LookAtJob lookAtJob = new LookAtJob
        {
            deltaTime = Time.deltaTime,
            playerPos = target
        };
        lookAtJob.ScheduleParallel();
    }

    [WithNone(typeof(Player))]
    public partial struct LookAtJob : IJobEntity
    {
        public float deltaTime;
        public float3 playerPos;
        public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
        {
            float3 targetPos;
            targetPos.x = playerPos.x;
            targetPos.y = playerPos.y;
            targetPos.z = playerPos.z;


            float3 curentPos;

            curentPos.x = localTransform.Position.x;
            curentPos.y = localTransform.Position.y;
            curentPos.z = localTransform.Position.z;

            float3 relativePos = targetPos - curentPos;

            // the second argument, upwards, defaults to Vector3.up

            //quaternion start = localTransform.Rotation;
            quaternion end = quaternion.LookRotation(relativePos, math.up());

            //quaternion result=quaternion.sl
            //localTransform.Rotation = end;

            localTransform.Rotation = end;
        }
    }
}
