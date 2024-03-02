using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial struct BulletSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
    }

    public void OnUpdate(ref SystemState state) {
        foreach (RefRW<Bullet> bullet in SystemAPI.Query<RefRW<Bullet>>())
        {

        }
    }
}
