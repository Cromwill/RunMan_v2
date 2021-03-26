using UnityEngine;
using System.Collections;
using System;

public class PlayerLife : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private float _defaultLife;
    [SerializeField] private float _defualtArmor;
    [SerializeField] private float _safeTime;
    [SerializeField] private BoosterType _type;
    [SerializeField] private ParticleSystem _damageEffect;

    private bool _isSafeTime;

    public BoosterType BoosterType => _type;

    private event Action<string> _boosterUsed;

    public void Initialization(Action<string> action, params Booster[] boosters)
    {
        _boosterUsed = action;

        if (boosters != null)
        {
            foreach (Booster booster in boosters)
                UseBooster(booster);
        }
    }

    public void UsedSkill(SkillData skill, int count)
    {

    }

    public bool ReduceLife(int damage)
    {
        int currentDamage = damage;

        if(_defualtArmor > 0)
        {
            currentDamage -= (int)_defualtArmor;
            _defualtArmor = 0;
        }
        if (_defaultLife > 1)
        {
            _defaultLife -= currentDamage;
        }
        else
        {
            _defaultLife -= currentDamage;
        }

        return _defaultLife <= 0;
    }

    private void UseBooster(Booster booster)
    {
        switch(booster.GetItemName)
        {
            case "Armor":
                _defualtArmor += booster.Value;
                _boosterUsed?.Invoke("Armor");
                break;
            case "Life":
                _defaultLife += booster.Value;
                _boosterUsed?.Invoke("Life");
                break;
        }
    }

}
