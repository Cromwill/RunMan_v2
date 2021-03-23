using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Skill", menuName = "SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField] private int _basePrice;
    [SerializeField] private float _incrementStep;
    [SerializeField] private SkillType _type;
    [SerializeField] private BoosterType _boosterType;

    [SerializeField] private string _saveKey;
    [SerializeField] private float _multiplier;
    [SerializeField] private int[] _prices;

    public string skillKey => _saveKey;
    public float Multiplier => _multiplier;

    public int GetPrice(int count)
    {
        if (count >= 0 && count < _prices.Length)
            return _prices[count];
        else
        {
            Debug.LogError("Count more then prices length");
            return 0;
        }
    }

    public void UseSkill(IPlayerComponent[] components)
    {
        if (SaveDataStorage.LoadSkills(skillKey) > 0)
        {
            foreach (var component in components)
            {
                if (component.BoosterType == _boosterType)
                    component.UsedSkill(this, SaveDataStorage.LoadSkills(skillKey));
            }
        }
    }
}
