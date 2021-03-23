using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class EnemiesConstructor : MonoBehaviour
{
    [SerializeField] private EnemiesSpawner _spawner;
    [SerializeField] private Vector3 _spawnRect;
    [SerializeField] private Enemy _ogre;
    [SerializeField] private EnemiesSpawner _rocket;

    private List<SpawnDotData> _currentSpawners = new List<SpawnDotData>();
    private IEnemySpawnGenerator _spawnGenerator;

    private void Start()
    {
        _spawnGenerator = new EnemySpawnGenerator(_spawnRect);
        StartCoroutine(GenerateOgre(10));
        StartCoroutine(GenerateRocket(15));
    }
    public void GenerateEnemeSpawners(TileGeneration[] tiles, TileGeneration currentTile)
    {
        if (_spawnGenerator == null)
            _spawnGenerator = new EnemySpawnGenerator(_spawnRect);

        var tilesToGenerate = _spawnGenerator.GetTilesToSpawn(tiles, currentTile);

        if (tilesToGenerate.Length > 0)
        {
            foreach (var tileToGenerate in tilesToGenerate)
            {
                SetSpawnerOnScene(tileToGenerate);
            }
        }
    }

    private IEnumerator GenerateOgre(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            TileGeneration tile = _spawnGenerator.GetTileForEnemy();
            Vector3 spawnPosition = tile.GetPosition();
            spawnPosition.y += 2.2f;
            Instantiate(_ogre, spawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }

    private IEnumerator GenerateRocket(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            TileGeneration tile = _spawnGenerator.GetTileForEnemy();
            Vector3 spawnPosition = tile.GetPosition();
            spawnPosition.y += 0.5f;
            Instantiate(_rocket, spawnPosition, Quaternion.identity);
        }
    }

    private void SetSpawnerOnScene(TileGeneration tile)
    {
        SpawnDotData spawner = new SpawnDotData(tile);
        spawner.Spawner = Instantiate(_spawner, tile.GetPosition(), Quaternion.identity);
        spawner.SetConnectionWithTile();
        _currentSpawners.Add(spawner);
    }
}

public class SpawnDotData
{
    public TileGeneration Tile;
    public EnemiesSpawner Spawner;

    public Vector3 tilePosition => Tile.GetPosition();

    public SpawnDotData(TileGeneration tile)
    {
        Tile = tile;
    }

    public bool IsInRange(Vector3 min, Vector3 max)
    {
        Vector3 position = Tile.GetPosition();

        return position.x >= min.x && position.x <= max.x && position.z <= max.z && position.z >= min.z;
    }

    public void SetConnectionWithTile()
    {
        if (Tile != null && Spawner != null)
        {
            Tile.AddSpawner(Spawner);
        }
    }
}
