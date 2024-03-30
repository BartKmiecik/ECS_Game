using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using UnityEngine;


public class JumpEnemyAuthoring : MonoBehaviour
{
    public float2 force; //Range
    public float2 cooldown; // Range
    private Unity.Mathematics.Random random;


    public class Baker : Baker<JumpEnemyAuthoring>
    {
        public override void Bake(JumpEnemyAuthoring authoring)
        {
            authoring.random = new Unity.Mathematics.Random((uint)(math.max(1,Time.deltaTime * 100000)));
            float f = authoring.random.NextFloat(authoring.force.x, authoring.force.y);
            float c = authoring.random.NextFloat(authoring.cooldown.x, authoring.cooldown.y);
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new JumpEnemy
            {
                force = f,
                cooldown = c,
                currentCooldown = c,
            });
        }
    }
}

public partial struct JumpEnemy : IComponentData
{
    public float force;
    public float cooldown;
    public float currentCooldown;
}
