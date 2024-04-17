using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class W1ReloadTimeSkill : MonoBehaviour, ISkill
{
    public float reloadReduction;
    public string description;
    public string Description => description;
    public Sprite icon;
    public Sprite Icon => icon;

    public void Skill()
    {
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<AmmoSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<AmmoSystem>(handle).Weapon1ReloadReuction(reloadReduction);
    }
}
