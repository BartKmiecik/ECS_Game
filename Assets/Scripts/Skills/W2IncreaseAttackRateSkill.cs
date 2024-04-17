using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class W2IncreaseAttackRateSkill : MonoBehaviour, ISkill
{
    private Entity _playerEntity;
    public float coolDownDecrease;
    public string description;
    public string Description => description;
    public Sprite icon;
    public Sprite Icon => icon;

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
        PlayerShootingV2 _playerShootingEntity = _entityManager.GetComponentData<PlayerShootingV2>(_playerEntity);
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerShootingSystemV2>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PlayerShootingSystemV2>(handle).UpdateCoolDown(coolDownDecrease);
    }
}
