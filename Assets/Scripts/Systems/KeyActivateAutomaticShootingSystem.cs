using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics.Systems;
using UnityEngine;

public partial struct KeyActivateAutomaticShootingSystem : ISystem
{
    private bool automaticShoting;

    public void ChangeSystemStates(bool state)
    {
        this.automaticShoting = state;
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerShootingSystemV2>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PlayerShootingSystemV2>(handle).automatic = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<ShotingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<ShotingSystem>(handle).automatic = state;

    }

    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log($"O pressed: {automaticShoting}");
            automaticShoting = !automaticShoting;
            ChangeSystemStates(automaticShoting);
        }
    }
}
