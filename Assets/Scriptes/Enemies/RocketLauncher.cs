using System.Collections;
using UnityEngine;

public class RocketLauncher : EnemiesSpawner
{
    [SerializeField] private Enemy _rocket;
    [SerializeField] private Transform _spawnDot;
    private Transform _playerTransform;

    void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
        StartCoroutine(SpawnEnemy());
    }

    void FixedUpdate()
    {
        transform.LookAt(_playerTransform);
    }

    protected override IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_spawnTime);
        Enemy enemy = Instantiate(_rocket, _spawnDot.position, _spawnDot.rotation);
        _playerFounded += enemy.SetPlayer;
    }
}
