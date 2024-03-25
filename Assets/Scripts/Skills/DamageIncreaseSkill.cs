using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class DamageIncreaseSkill : MonoBehaviour, ISkill
{
    private Entity _playerEntity;
    public int damageIncrease;
    public string description;

    public string Description => description;

    public void Skill()
    {
        EntityManager _entityManager;
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var entities = _entityManager.GetAllEntities(Allocator.Temp);
        foreach (var entity in entities)
        {
            if (_entityManager.HasComponent<Player>(entity))
            {
                _playerEntity = entity;
                break;
            }
        }
        PlayerShooting _playerControl = _entityManager.GetComponentData<PlayerShooting>(_playerEntity);
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<ShotingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<ShotingSystem>(handle).UpdateDamage(damageIncrease);
    }
}
