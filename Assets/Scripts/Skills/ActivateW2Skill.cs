using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ActivateW2Skill : MonoBehaviour, ISkill
{
    public string description;
    public string Description => description;

    public Sprite icon;
    public Sprite Icon => icon;

    public List<ISkill> _newWeaponSkills = new List<ISkill>();
    private UIExpBar _expBar;
    public GameObject uiActiveSkill;

    private void Start()
    {
        _newWeaponSkills.Clear();
        _newWeaponSkills.AddRange(GetComponents<ISkill>());
        _newWeaponSkills.Remove(this);
        _expBar = transform.parent.GetComponent<UIExpBar>();
        _expBar.AddNewSkill(this);
    }

    public void Skill()
    {
        var handle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerShootingSystemV2>();
        World.DefaultGameObjectInjectionWorld.Unmanaged.GetUnsafeSystemRef<PlayerShootingSystemV2>(handle).EnableWeapon(true);
        _expBar.AddNewSkills(_newWeaponSkills);
        _expBar.RemoveSkill(this);
        uiActiveSkill.SetActive(true);
        Destroy(gameObject);
    }
}
