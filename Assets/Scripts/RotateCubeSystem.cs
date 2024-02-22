using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;

[BurstCompile]
public partial struct RotateCubeSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }

    
    void OnUpdate(ref SystemState state) {
        /*
        foreach ((RefRW<LocalTransform> localTransform, RefRO<RotateSpeed> rotateSpeed) in SystemAPI.Query<RefRW<LocalTransform>,RefRO<RotateSpeed>>())
        {
            localTransform.ValueRW = localTransform.ValueRO.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime);
        }
        */
        RotatingCubeJob rotatingCubeJob = new RotatingCubeJob
        {
            deltaTime = Time.deltaTime
        };
        //rotatingCubeJob.Schedule();
        //state.Dependency = rotatingCubeJob.Schedule(state.Dependency);
        rotatingCubeJob.ScheduleParallel();
    }

    public partial struct RotatingCubeJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
        {
            localTransform = localTransform.RotateY(rotateSpeed.value * deltaTime);
        }
    }
}
