using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[BurstCompile]
public partial struct PlayerControllingSystem : ISystem
{
    public bool paused;
    float horizontal, vertical;

    private float extraSpeed;

    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerControl>();
    }

    void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (extraSpeed > 0)
        {
            ShadowMovement simpleShadow = new ShadowMovement
            {
                deltaTime = Time.deltaTime,
                horizontal = horizontal,
                vertical = vertical,
                extraSpeed = extraSpeed,
            };
            simpleShadow.ScheduleParallel();
            extraSpeed = 0;
        }
        else
        {
            ShadowMovement simpleShadow = new ShadowMovement
            {
                deltaTime = Time.deltaTime,
                horizontal = horizontal,
                vertical = vertical,
                extraSpeed = extraSpeed,
            };
            simpleShadow.ScheduleParallel();
        }
    }

    public void UpdatePlayerSpeed(float speed)
    {
        extraSpeed = speed;
    }

    [WithAny(typeof(Player))]
    //[WithAll(typeof(LookAt), typeof(MovementSpeed))]
    public partial struct ShadowMovement : IJobEntity
    {
        public float deltaTime;
        public float horizontal;
        public float vertical;
        public float extraSpeed;

        public void Execute(ref LocalTransform localTransform, ref PlayerControl playerControl)
        {
            if (extraSpeed > 0)
            {
                playerControl.speed += extraSpeed;
                extraSpeed = 0;
            }

            float3 targetPos;
            targetPos.x = localTransform.Position.x + (horizontal * playerControl.speed * deltaTime);
            targetPos.y = localTransform.Position.y;
            targetPos.z = localTransform.Position.z + (vertical * playerControl.speed * deltaTime);

            localTransform.Position = targetPos;
        }
    }
}
