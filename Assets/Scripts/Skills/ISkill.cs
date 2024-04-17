using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public string Description { get; }
    public Sprite Icon { get; }
    public void Skill();
}
