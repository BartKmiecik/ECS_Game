using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SkillIncreaseAttackRate : MonoBehaviour, ISkill
{
    private Entity _playerEntity;

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
        PlayerShooting _playerShootingEntity = _entityManager.GetComponentData<PlayerShooting>(_playerEntity);
        float tmp = _playerShootingEntity.cooldown + 2f;
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<ShotingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<ShotingSystem>(handle).UpdateCoolDown(tmp);
    }
}
