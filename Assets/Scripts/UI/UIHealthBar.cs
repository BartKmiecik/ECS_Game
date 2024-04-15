using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private EntityManager _entityManager;
    private Image _image;
    private Entity _playerEntity;
    private Player _player;
    private float _curentHealth = 0;
    public GameObject gameOverScreen;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
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
    }

    void LateUpdate()
    {
        if (_entityManager.Exists(_playerEntity))
        {
            _player = _entityManager.GetComponentData<Player>(_playerEntity);
            _curentHealth = (float)_player.currentHealth / (float)_player.maxHealth;
            _image.fillAmount = _curentHealth;
            if (_curentHealth <= 0)
            {
                gameOverScreen.SetActive(true);
                var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PauseGameSystem>();
                World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PauseGameSystem>(handle).ChangeSystemStates(true, false);
            }
        }
        else
        {
            Init();
        }
    }
}
