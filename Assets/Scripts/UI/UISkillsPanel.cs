using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Entities;
using Unity.Collections;

public class UISkillsPanel : MonoBehaviour
{
    public Image skill1Cooldown;
    public TextMeshProUGUI skill1Ammo;
    public Image skill2Cooldown;
    public TextMeshProUGUI skill2Ammo;

    private EntityManager _entityManager;
    private Entity _playerEntity;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
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
    }

    void Update()
    {
        if (_entityManager.Exists(_playerEntity))
        {
            var shooting1 = _entityManager.GetComponentData<PlayerShooting>(_playerEntity);
            skill1Ammo.text = ($"{shooting1.currentAmo} / {shooting1.maxAmo + shooting1.extraAmo}");
            if (shooting1.currentAmo == 0)
            {
                skill1Cooldown.enabled = true;
                skill1Cooldown.fillAmount = shooting1.currentReload / shooting1.maxReloadSpeed;
            }
            else
            {
                skill1Cooldown.enabled = false;
            }
            var shooting2 = _entityManager.GetComponentData<PlayerShootingV2>(_playerEntity);
            skill2Ammo.text = ($"{shooting2.currentAmo} / {shooting2.maxAmo + shooting2.extraAmo}");
            if (shooting2.currentAmo == 0)
            {
                skill2Cooldown.enabled = true;
                skill2Cooldown.fillAmount = shooting2.currentReload / shooting2.maxReloadSpeed;
            }
            else
            {
                skill2Cooldown.enabled = false;
            }
        }
        else
        {
            Init();
        }


    }
}
