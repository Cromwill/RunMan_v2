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
    
    private FogPool _pool;

    private LevelConstructor _levelConstructor;
    private float _fogZPosition;
    private float _fogStep;
    private List<Fog> _currentFog = new List<Fog>();

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
