using Unity.Entities;
using Unity.Physics.Systems;
using UnityEngine;


public partial struct PauseGameSystem : ISystem
{
    private bool isPaused;

    public void ChangeSystemStates(bool state)
    {
        this.isPaused = state;
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UIBridge>().Paused(state);

        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimpleSpawnSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<SimpleSpawnSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimpleChasingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<SimpleChasingSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerControllingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PlayerControllingSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<LifespanSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<LifespanSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BulletSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<BulletSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<ShotingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<ShotingSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerShootingSystemV2>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PlayerShootingSystemV2>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<RotateFlameSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<RotateFlameSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<LookAtMouseSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<LookAtMouseSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<LookAtSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<LookAtSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<CollectableSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<CollectableSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<DamagePlayerSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<DamagePlayerSystem>(handle).paused = state;

        handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<EnemySimpleShotingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<EnemySimpleShotingSystem>(handle).paused = state;

        World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PhysicsSystemGroup>().Enabled = !state;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            Debug.Log($"P pressed: {isPaused}");
            isPaused = !isPaused;
            ChangeSystemStates(isPaused);
        }
    }
}

