using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct FollowFlowFieldSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        FollowFlowFieldJob followJob = new FollowFlowFieldJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        };
        followJob.ScheduleParallel();
    }

    public partial struct FollowFlowFieldJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref LocalTransform localTransform, in FollowFlowField _, in MovementSpeed movementSpeed)
        {
            Cell cellBelow = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BridgeGridFollowSystem>()
                .GetCellFromWorldPos(localTransform.Position);
            Vector3 moveDirection = new Vector3(cellBelow.bestDirection.Vector.x, 0, cellBelow.bestDirection.Vector.y);
            localTransform.Position += new float3(moveDirection) * deltaTime * movementSpeed.speed;
        }
    }
}

