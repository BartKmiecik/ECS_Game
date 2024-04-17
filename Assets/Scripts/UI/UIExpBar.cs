using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    private ISkill tempSkill = null;

    public System.Random a = new System.Random();
    private List<int> randomSkillList = new List<int>();
    private int counter = 0;
    void Start()
    {
        Init();
    }

    private void Init()
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
        if (_skills_to.Count > 0)
        {
            return;
        }
        _skills_to.AddRange(GetComponents<ISkill>());
        if (tempSkill != null)
        {
            _skills_to.Add(tempSkill);
            tempSkill = null;
        }
    }

    public void AddNewSkills(List<ISkill> skills)
    {
        _skills_to.AddRange(skills);
    }

    public void AddNewSkill(ISkill skill)
    {
        if (_skills_to.Count > 0)
        {
            _skills_to.Add(skill);
        }
        else
        {
            tempSkill = skill;
        }
    }

    public void RemoveSkill(ISkill skill)
    {
        _skills_to.Remove(skill);
    }

    private void GetRandom()
    {
        int MyNumber = a.Next(0, _skills_to.Count);
        if (!randomSkillList.Contains(MyNumber))
            randomSkillList.Add(MyNumber);
    }

    private void ShowAllSkills()
    {
        foreach (ISkill skill in _skills_to)
        {
            Debug.Log(skill.ToString());
        }
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
                randomSkillList.Clear();
                while (randomSkillList.Count < 3) 
                {
                    GetRandom();
                }
                foreach (int i in randomSkillList)
                {
                    GameObject _skill = Instantiate(skillUiPrefab, levelUpPanel);
                    _skill.GetComponent<UISkillSelect>().Constructor(_skills_to[i].Icon, _skills_to[i].Description, i, this);
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
        for (int i = skills.Count - 1; i >=0; i--)
        {
            Destroy(skills[i]);
        }
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PauseGameSystem>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PauseGameSystem>(handle).ChangeSystemStates(false, false);
    }
}
