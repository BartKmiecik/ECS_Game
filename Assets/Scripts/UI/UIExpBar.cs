using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UIExpBar : MonoBehaviour
{
    private EntityManager _entityManager;
    private Image _image;
    private Entity _playerEntity;
    private Player _player;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var playerControlSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerControllingSystem>();
        _image = GetComponent<Image>();
        var entities = _entityManager.GetAllEntities(Allocator.Temp);
        foreach (var entity in entities)
        {
            if (_entityManager.HasComponent<Player>(entity))
            {
                _playerEntity = entity;
                break;
            }
        }
        _player = _entityManager.GetComponentData<Player>(_playerEntity);
    }

    void LateUpdate()
    {
        _player = _entityManager.GetComponentData<Player>(_playerEntity);
        _image.fillAmount = ((float)_player.expirience % 100)/100;
    }
}
