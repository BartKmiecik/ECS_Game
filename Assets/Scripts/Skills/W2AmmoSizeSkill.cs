using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class W2AmmoSizeSkill : MonoBehaviour, ISkill
{
    public int extraAmmo;
    public string description;
    public string Description => description;
    public Sprite icon;
    public Sprite Icon => icon;

    public void Skill()
    {
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<AmmoSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<AmmoSystem>(handle).Weapon2IncreaseAmmo(extraAmmo);
    }
}
