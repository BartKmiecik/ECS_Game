using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
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

        public void Execute(ref PhysicsVelocity physicsVelocity, ref PlayerControl playerControl)
        {
            if (extraSpeed > 0)
            {
                playerControl.maxSpeed += extraSpeed;
                extraSpeed = 0;
            }

            float2 targetPos;
            targetPos.x = horizontal * playerControl.acceleration;
            targetPos.y = vertical * playerControl.acceleration;

/*            float3 currentSpped = targetPos * deltaTime;*/

            if (math.sign(targetPos.x) == math.sign(physicsVelocity.Linear.x))
            {
                physicsVelocity.Linear.x += targetPos.x * deltaTime;
            }  else if (math.sign(targetPos.x) == 0)
            {

            }else
            {
                physicsVelocity.Linear.x = 0;
                physicsVelocity.Linear.x += targetPos.x * deltaTime;
            }
            if (math.sign(targetPos.y) == math.sign(physicsVelocity.Linear.z))
            {
                physicsVelocity.Linear.z += targetPos.y * deltaTime;
            }
            else if (math.sign(targetPos.y) == 0)
            {
                
            }
            else
            {
                physicsVelocity.Linear.z = 0;
                physicsVelocity.Linear.z += targetPos.y * deltaTime;
            }


            if (math.abs(physicsVelocity.Linear.x) > playerControl.maxSpeed)
            {
                physicsVelocity.Linear.x -= targetPos.x * deltaTime;
            } 
            if (math.abs(physicsVelocity.Linear.z) > playerControl.maxSpeed)
            {
                physicsVelocity.Linear.z -= targetPos.y * deltaTime;
            }
        }
    }
}
