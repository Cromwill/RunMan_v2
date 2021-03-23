using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LevelConstructor))]
public class FogConstructor : MonoBehaviour
{
    [SerializeField] private int _startOfsetPosition;
    [SerializeField] private Fog _prefab;
    [SerializeField] private float _fogRegenirationTime;
    [SerializeField] private float _fogLifeTime;
    [SerializeField] private float _lifeTime;
    [SerializeField] private Player _player;
    
    private FogPool _pool;

    private LevelConstructor _levelConstructor;
    private float _fogZPosition;
    private float _fogStep;
    private List<Fog> _currentFog = new List<Fog>();

    private const float _normalFogSpeed = 4.0f;
    private const float _fastFogSpeed = 2.75f;
    private const float _fastestFogSpeed = 1.0f;

    public void StartGenerate(LevelConstructor levelConstructor, TileGeneration startTile)
    {
        _levelConstructor = levelConstructor;
        _fogZPosition = startTile.GetPosition().z - startTile.GetSize().z * _startOfsetPosition;
        _fogStep = startTile.GetSize().z;
        _pool = FindObjectOfType<FogPool>();

        GenerateFog();
        StartCoroutine(NextGnerateFog());
    }

    private IEnumerator NextGnerateFog()
    {
        while(true)
        {
            yield return new WaitForSeconds(_fogRegenirationTime);
            _fogZPosition += _fogStep;
            float distance = _player.transform.position.z - _fogZPosition;
            if (distance > 50.0f)
                _fogRegenirationTime = _fastestFogSpeed;
            else if (distance > 20.0f)
                _fogRegenirationTime = _fastFogSpeed;
            else
                _fogRegenirationTime = _normalFogSpeed;

            GenerateFog();
        }
    }

    private void GenerateFog()
    {
        var tiles = _levelConstructor.GetTiles().Where(t => t.GetPosition().z <= _fogZPosition && t.IsHaveFog == false && t.IsInThePool == false);

        foreach (var tile in tiles)
        {
            var fog = _pool.GetFog();
            fog.Initialization(tile, _fogLifeTime, _lifeTime);
            tile.AddFog(fog);
            _currentFog.Add(fog);
            fog.Destriction += delegate { _currentFog.Remove(fog); };
            fog.Destriction += delegate { _pool.ReturnToPool(fog); };
        }
    }
}
