using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    private EntityManager _entityManager;
    private Image _image;
    private Entity _playerEntity;
    private Player _player;
    [SerializeField] private Transform levelUpPanel;
    [SerializeField] private GameObject skillUiPrefab;
    private List<GameObject> skills = new List<GameObject>();
    public int skills_to_select = 3;
    private float _expirience = 0;
    private float _lastExpirience = 0;
    private List<ISkill> _skills_to = new List<ISkill>();

    public System.Random a = new System.Random();
    private List<int> randomSkillList = new List<int>();

    void Start()
    {
        Init();
    }

    private void Init()
    {
        _skills_to.Clear();
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
        _skills_to.AddRange(GetComponents<ISkill>());
    }

    private void GetRandom()
    {
        int MyNumber = a.Next(0, _skills_to.Count);
        if (!randomSkillList.Contains(MyNumber))
            randomSkillList.Add(MyNumber);
    }

    void LateUpdate()
    {
        if (_entityManager.Exists(_playerEntity))
        {
            _player = _entityManager.GetComponentData<Player>(_playerEntity);
            _lastExpirience = _expirience;
            _expirience = ((float)_player.expirience % 100) / 100;
            _image.fillAmount = _expirience;

            if (_lastExpirience > _expirience)
            {
                var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PauseGameSystem>();
                World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PauseGameSystem>(handle).ChangeSystemStates(true, false);
                skills.Clear();
                /*for (int i = 0; i < skills_to_select; i++)
                {
                    GameObject _skill = Instantiate(skillUiPrefab, levelUpPanel);
                    _skill.GetComponent<UISkillSelect>().Constructor(null, _skills_to[i].Description, i, this);
                    skills.Add(_skill);
                }*/
                randomSkillList.Clear();
                while (randomSkillList.Count < 3) 
                {
                    GetRandom();
                }
                foreach (int i in randomSkillList)
                {
                    GameObject _skill = Instantiate(skillUiPrefab, levelUpPanel);
                    _skill.GetComponent<UISkillSelect>().Constructor(null, _skills_to[i].Description, i, this);
                    skills.Add(_skill);
                }
            }
        }
        else
        {
            Init();
        }

    }

    public void OnSkillSelected(int skillSelected)
    {
        _skills_to[skillSelected].Skill();
        Debug.Log($"Skill selected: {skillSelected}");
        for (int i = skills.Count - 1; i >=0; i--)
        {
            Destroy(skills[i]);
        }
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PauseGameSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PauseGameSystem>(handle).ChangeSystemStates(false, false);
    }
}
