using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Restart()
    {
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PauseGameSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PauseGameSystem>(handle).ChangeSystemStates(false, false);
        World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(World.DefaultGameObjectInjectionWorld.EntityManager.UniversalQuery);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entityArray = entityManager.GetAllEntities();
        foreach (var e in entityArray)
            entityManager.DestroyEntity(e);
        entityArray.Dispose();
        World.DisposeAllWorlds();
        DefaultWorldInitialization.Initialize("Base World", false);
        ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(sceneName);

        /*Time.timeScale = .5f;*/
    }

    public void Menu()
    {
        //TODO
    }
}
