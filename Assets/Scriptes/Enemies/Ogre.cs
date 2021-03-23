using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Enemy, ISearchablePlayers
{
    private bool _isStan;

    private void Start()
    {
        _selfRigidbody = GetComponent<Rigidbody>();
        _selfAnimator = GetComponent<Animator>();
        _turnSpeed = _maxTurnSpeed;
    }

    private void FixedUpdate()
    {
        if (_isPlayerFounded && !_isStan)
        {
            Vector3 direction = _isPlayerFounded ? _player.position : transform.forward + new Vector3(10, 0, 0);

            Turn(direction);
            _selfRigidbody.velocity = transform.forward.normalized * _speed * Time.fixedDeltaTime;
        }
    }

    public override void SetPlayer(Player player)
    {
        base.SetPlayer(player);
        _selfAnimator.SetBool("Run", true);
        _isPlayerFounded = true;
    }

    public override void Dead()
    {
        _isDead = true;
        //_selfAnimator.SetBool("Death", true);
        _selfAnimator.SetTrigger("Death");
        StartCoroutine(StartEffect());
        base.Dead();
    }

    public void OnPlayerFounded(Player player) => SetPlayer(player);

    private IEnumerator StartEffect()
    {
        yield return new WaitForSeconds(1);
        if (_deadEffect != null)
            Instantiate(_deadEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void Turn(Vector3 target)
    {
        target = new Vector3(target.x, target.y + 1.0f, target.z);
        Vector3 direction = target - transform.position;
        float step = _turnSpeed * Time.fixedDeltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction.normalized, step, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red, 2.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public override void AddDamage(float damage)
    {
        _selfAnimator.SetTrigger("Roar");
        _isStan = true;
        StartCoroutine(ReturnFromStan());
        base.AddDamage(damage);
    }

    private IEnumerator ReturnFromStan()
    {
        yield return new WaitForSeconds(1);
        _isStan = false;
    }
}
