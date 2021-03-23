using System;

public interface IPlayerComponent
{
    void Initialization(Action<string> usedBoosterEvent, params Booster[] boosters);
    BoosterType BoosterType { get; }
    void UsedSkill(SkillData skill, int count);
}

