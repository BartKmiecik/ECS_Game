using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct LookAtMouseSystem : ISystem
{
    public bool paused;
    Vector3 mid_screen;
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<LookAtMouse>();
        mid_screen = new Vector3 (0.5f, 0.5f, 0.0f);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.x - b.x, a.y - b.y) * Mathf.Rad2Deg;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (paused) return;
        foreach ((RefRW<LocalTransform> localTransform, RefRO<Player> _) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Player>>())
        {
            Camera cam = Camera.main;
            float3 mousePos = cam.ScreenToViewportPoint(Input.mousePosition);

            float3 targetPos;
            targetPos.x = mousePos.x;
            targetPos.y = mousePos.y;
            targetPos.z = mousePos.z;

            float angle = AngleBetweenTwoPoints(mid_screen, mousePos)-180;

            /*            float3 curentPos;

                        curentPos.x = localTransform.ValueRO.Position.x;
                        curentPos.y = localTransform.ValueRO.Position.y;
                        curentPos.z = localTransform.ValueRO.Position.z;*/

            //float3 relativePos = targetPos - playerPos;

            // the second argument, upwards, defaults to Vector3.up

            //quaternion start = localTransform.Rotation;
            quaternion end = Quaternion.Euler(new Vector3(0f, angle, 0f));
            

            //quaternion result=quaternion.sl
            //localTransform.Rotation = end;

            localTransform.ValueRW.Rotation = end;
        }
    }
}

