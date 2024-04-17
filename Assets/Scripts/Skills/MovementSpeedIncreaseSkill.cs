using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class MovementSpeedIncreaseSkill : MonoBehaviour, ISkill
{
    private Entity _playerEntity;
    public float movementIcrease;
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
        PlayerControl _playerControl = _entityManager.GetComponentData<PlayerControl>(_playerEntity);
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerControllingSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PlayerControllingSystem>(handle).UpdatePlayerSpeed(movementIcrease);
    }
}
