using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;

[UpdateAfter(typeof(SimulationSystemGroup))]
public partial struct AnimationSystem : ISystem
{
    public void OnCreation(ref SystemState state)
    {
        state.RequireForUpdate<PlayerAnimatorReference>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (playerGameObjectPrefab, transform, entity) in
                 SystemAPI.Query<PlayerGameObjectPrefab, LocalTransform>().WithNone<PlayerAnimatorReference>().WithEntityAccess())
        {
            var newCompanionGameObject = Object.Instantiate(playerGameObjectPrefab.Value);
            var newAnimatorReference = new PlayerAnimatorReference
            {
                Value = newCompanionGameObject.GetComponent<Animator>()
            };
            newAnimatorReference.Value.transform.position = transform.Position;
            newAnimatorReference.Value.transform.rotation = transform.Rotation;
            ecb.AddComponent(entity, newAnimatorReference);
        }

        foreach (var (transform, animatorReference) in
                 SystemAPI.Query<LocalTransform, PlayerAnimatorReference>())
        {
            animatorReference.Value.transform.position = transform.Position;
            animatorReference.Value.transform.rotation = transform.Rotation;
        }

        foreach (var (transform, animatorReference, dashingEnemy) in
                 SystemAPI.Query<LocalTransform, PlayerAnimatorReference, DashingEnemy>())
        {
            animatorReference.Value.SetBool("IsMoving", dashingEnemy.currentCooldown <= .1f);
        }

        foreach (var (animatorReference, entity) in
                 SystemAPI.Query<PlayerAnimatorReference>().WithNone<PlayerGameObjectPrefab, LocalTransform>()
                     .WithEntityAccess())
        {
            Object.Destroy(animatorReference.Value.gameObject);
            ecb.RemoveComponent<PlayerAnimatorReference>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
