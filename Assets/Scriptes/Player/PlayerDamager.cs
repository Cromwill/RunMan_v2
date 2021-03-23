using System;
using System.Collections;
using UnityEngine;

public class PlayerDamager : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private int _bulletCount;
    [SerializeField] private ArmorInfo _armorViewer;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private float _defaultDamage;
    [SerializeField] private BoosterType _type;

    private PlayerDamageZone _damageZone;
    private Animator _playerAnimator;

    public event Action BulletsRunOut;

    public BoosterType BoosterType => _type;

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _damageZone = GetComponentInChildren<PlayerDamageZone>();
        _damageZone.FindedEnemies += Shoot;
        if (_armorViewer != null)
            _armorViewer.Show(_bulletCount);
    }

    public void Initialization(Action<string> action, params Booster[] boosters)
    {
        if (boosters != null)
        {
            foreach (Booster booster in boosters)
                UsedBooster(booster);
        }
    }

    public void UsedSkill(SkillData skill, int count)
    {
        switch (skill.skillKey)
        {
            case "Damage":
                _defaultDamage *= (skill.Multiplier * count);
                break;
            case "RateOfFire":
                _shootSpeed *= (skill.Multiplier * count);
                break;
        }
    }

    private void UsedBooster(Booster booster)
    {
        switch (booster.GetItemName)
        {
            case "Damage":
                _defaultDamage += booster.Value;
                break;
        }
    }

    private void Shoot(Enemy enemy)
    {
        if (_bulletCount > 0)
        {
            _bulletCount--;
            _armorViewer.Show(_bulletCount);
            enemy.AddDamage(_defaultDamage);
            _playerAnimator.SetBool("Shoot", true);
            StartCoroutine(FinishShoot());
        }
    }

    private IEnumerator FinishShoot()
    {
        yield return new WaitForSeconds(0.25f);
        _playerAnimator.SetBool("Shoot", false);
    }
}
