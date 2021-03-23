using UnityEngine;
using System.Collections;
using System;

public class PlayerMover : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private float _accelerationTime;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _currentRotationSpeed;
    [SerializeField] private float _maxRotationSpeed;
    [SerializeField] private BoosterType _type;

    private Player _player;
    private float _speed;
    private float _rotationSpeed;
    private Rigidbody _selfRigidbody;
    private Animator _selfAnimator;

    public BoosterType BoosterType => _type;

    private void Start()
    {
        _selfRigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
        _selfAnimator = GetComponent<Animator>();
        _rotationSpeed = _currentRotationSpeed;
        _speed = (_maxSpeed + _minSpeed) / 2;

        _player.Deading += delegate { _speed = 0; };
    }

    public void Turn(RotateDirection direction)
    {
        float angle = (_rotationSpeed * Time.deltaTime) * (int)direction;
        transform.Rotate(new Vector3(0, angle, 0), Space.Self);
    }

    public void Move()
    {
        if (_speed < _maxSpeed)
        {
            _speed += (_maxSpeed - _minSpeed) / _accelerationTime * Time.fixedDeltaTime;
            if (_speed >= _maxSpeed) _speed = _maxSpeed;
        }

        _selfRigidbody.velocity = transform.forward.normalized * _speed * Time.fixedDeltaTime;
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
            case "RunningSpeed":
                _maxSpeed *= ((skill.Multiplier - 1) * count) + 1;
                break;
            case "SwingSpeed":
                _currentRotationSpeed *= ((skill.Multiplier - 1) * count) + 1;
                break;
        }
    }

    public void UseMaxRotationSpeed() => _rotationSpeed = _maxRotationSpeed;
    public void ResetRotationSpeed() => _rotationSpeed = _currentRotationSpeed;
    public void ResetSpeed() => _speed = _minSpeed;

    private void UsedBooster(Booster booster)
    {
        _minSpeed += booster.Value;
        _maxSpeed += booster.Value;
    }
}
