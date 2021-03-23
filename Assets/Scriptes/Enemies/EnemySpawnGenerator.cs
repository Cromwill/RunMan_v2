using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemySpawnGenerator : IEnemySpawnGenerator
{
    private Vector3 _spawnRect;
    private List<SpawnerRange> _tilesWithSpawner;
    private TileGeneration[] _currentTiles;
    private TileGeneration _currentTile;
    private int _stepX;
    private int _stepZ;

    public EnemySpawnGenerator(Vector3 spawnRect)
    {
        _spawnRect = spawnRect;
        _tilesWithSpawner = new List<SpawnerRange>();
        _stepX = (int)_spawnRect.x + 1;
        _stepZ = (int)_spawnRect.z + 1;
    }

    public TileGeneration[] GetTilesToSpawn(TileGeneration[] tiles, TileGeneration currentTile)
    {
        CheckTilesOnScene(tiles);
        _currentTile = currentTile;
        Vector3 size = _currentTile.GetSize();
        List<TileGeneration> tilesToSpawn = new List<TileGeneration>();
        _currentTiles = tiles;

        Vector3 startTile = _currentTile.GetPosition() + new Vector3(0, 0, _stepZ * size.z);
        tilesToSpawn.Add(_currentTiles.Where(a => a.GetPosition() == startTile).First());

        int counter = 1;
        bool isTileNotEnded = true;
        while (isTileNotEnded)
        {
            Vector3 nextPosition = new Vector3(startTile.x, startTile.y, startTile.z + _stepZ * counter * size.z);
            isTileNotEnded = AddTileByCoordinates(nextPosition, ref tilesToSpawn);
            counter++;

            if (counter > 100)
            {
                Debug.Log("counter > 100");
                isTileNotEnded = false;
            }
        }

        Vector3[] centralTiles = new Vector3[tilesToSpawn.Count];
        int tilesCounter = 0;

        foreach (var centralTile in tilesToSpawn)
        {
            centralTiles[tilesCounter] = centralTile.GetPosition();
            tilesCounter++;
        }

        tilesToSpawn = new List<TileGeneration>();

        for (int i = 0; i < centralTiles.Length; i++)
        {
            AddTileByCoordinates(centralTiles[i], ref tilesToSpawn);

            for (int j = -2; j <= 2; j++)
            {
                if (j != 0)
                {
                    Vector3 nextPosition = new Vector3(centralTiles[i].x + (j * _stepX * size.x), centralTiles[i].y, centralTiles[i].z);
                    AddTileByCoordinates(nextPosition, ref tilesToSpawn);
                }
            }
        }

        CheckSpawnsOnSceene(ref tilesToSpawn);

        if (tilesToSpawn.Count > 0)
        {
            foreach (var tileToSpawn in tilesToSpawn)
            {
                _tilesWithSpawner.Add( new SpawnerRange(tileToSpawn, _stepX, _stepZ));
            }
        }

        return tilesToSpawn.ToArray();
    }

    public TileGeneration GetTileForEnemy()
    {
        TileGeneration[] tiles = _currentTiles.Where(a => CheckEnemyPosition(a.GetPosition(), _stepZ * 2, _stepX) && !a.IsHaveSpawner).ToArray();

        return tiles[Random.Range(0, tiles.Length)];
    }


    private void CheckTilesOnScene(TileGeneration[] tiles)
    {
        List<SpawnerRange> removeList = new List<SpawnerRange>();
        if (_tilesWithSpawner != null)
        {
            foreach (var tileOnScene in _tilesWithSpawner)
            {
                if (tiles.Contains(tileOnScene.getTile) == false)
                    removeList.Add(tileOnScene);
            }

            foreach(var remove in removeList)
            {
                _tilesWithSpawner.Remove(remove);
            }
        }
    }

    private bool CheckEnemyPosition(Vector3 position, int stepZ, int stepX)
    {
        Vector3 currentPosition = _currentTile.GetPosition();
        bool check = position.z >= currentPosition.z + stepZ * _currentTile.GetSize().z &&
            position.x >= currentPosition.x - stepX * _currentTile.GetSize().x &&
            position.x <= currentPosition.x + stepX * _currentTile.GetSize().x;

        return check;
    }

    private void CheckSpawnsOnSceene(ref List<TileGeneration> tiles)
    {
        List<TileGeneration> removeList = new List<TileGeneration>();
        foreach(var tileOnScene in _tilesWithSpawner)
        {
            foreach(var tileToSpawn in tiles)
            {
                if(tileOnScene.IsInTheRange(tileToSpawn.GetPosition()))
                {
                    removeList.Add(tileToSpawn);
                }
            }
        }

        foreach (var tile in removeList)
            tiles.Remove(tile);
    }

    private bool AddTileByCoordinates(Vector3 position, ref List<TileGeneration> tiles)
    {
        TileGeneration tile = _currentTiles.Where(a => a.GetPosition() == position).FirstOrDefault();
        if (tile != null)
            tiles.Add(tile);

        return tile != null;
    }
}

