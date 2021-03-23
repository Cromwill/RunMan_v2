using UnityEngine;
using System.Collections;

public class Zombie : Enemy
{
    private void Start()
    {
        _selfRigidbody = GetComponent<Rigidbody>();
        _selfAnimator = GetComponent<Animator>();
        _selfAnimator.SetBool("Stop", true);
        _turnSpeed = Random.Range(_minTurnSpeed, _maxTurnSpeed);
    }

    private void FixedUpdate()
    {
        if (_isPlayerFounded && !_isDead)
        {
            Turn();
            _selfRigidbody.velocity = transform.forward.normalized * _speed * Time.fixedDeltaTime;
        }
    }

    public override void SetPlayer(Player player)
    {
        base.SetPlayer(player);
        StartCoroutine(ReactionToPlayer(Random.Range(0, _reactionTime)));
    }

    public override void Dead()
    {
        _isDead = true;
        _selfAnimator.SetBool("Death", true);
        StartCoroutine(StartEffect());
        base.Dead();
    }

    private IEnumerator ReactionToPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        _selfAnimator.SetBool("Run", true);
        _isPlayerFounded = true;
    }

    private IEnumerator StartEffect()
    {
        yield return new WaitForSeconds(1);
        if (_deadEffect != null)
            Instantiate(_deadEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void Turn()
    {
        Vector3 direction = _player.transform.position - transform.position;
        float step = _turnSpeed * Time.fixedDeltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction.normalized, step, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red, 2.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}