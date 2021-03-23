using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapElementPool : MonoBehaviour
{
    public List<MapElement> NonDestroyObjects;
    public List<MapElement> DestroyObjects;
    public List<TileGeneration> Tiles;
    public int TilesCount;
    public int StartTilesCount;

    private TileGeneration[] _tilePool;
    private ExitPanel _exitPanel;

    private void Awake()
    {
        _tilePool = FindObjectsOfType<TileGeneration>();
    }

    public IMapElement GetNonDestroyObject(Transform parent)
    {
        IMapElement element = Instantiate(NonDestroyObjects[Random.Range(0, NonDestroyObjects.Count)], parent);
        element.RandomRotate();
        return element;
    }

    public IMapElement GetNonDestroyObject()
    {
        IMapElement element = NonDestroyObjects[Random.Range(0, NonDestroyObjects.Count)];
        element.RandomRotate();
        return element;
    }
    public IMapElement GetDestroyObject()
    {
        IMapElement element = DestroyObjects[Random.Range(0, DestroyObjects.Count)];
        element.RandomRotate();
        return element;
    }

    public void SetExitPanel(ExitPanel exit)
    {
        _exitPanel = exit;
        exit.OnExit += ClearMap;
    }

    public TileGeneration GetTile()
    {
        var tilesThatAtPool = _tilePool.Where(t => t.IsInThePool == true).ToArray();
        TileGeneration tile;
        try
        {
            tile = tilesThatAtPool[Random.Range(0, tilesThatAtPool.Length)];
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("");
            return null;
        }

        tile.IsInThePool = false;
        return tile;
    }

    public TileGeneration GetTile(Vector3 position)
    {
        try
        {
            return _tilePool.Where(a => a.GetPosition() == position).First();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            Debug.LogError(ex.StackTrace);
            return null;
        }
    }

    public bool IsPositionEmpty(Vector3 position)
    {
        return _tilePool.Where(a => a.GetPosition() == position).Count() == 0;
    }

    public void ReturnToPool(TileGeneration tile)
    {
        Vector3 position = transform.position;

        for (int i = 0; i < _tilePool.Length; i++)
        {
            if (_tilePool[i] == tile)
            {
                _tilePool[i].ReturnToPool(new Vector3(position.x, position.y - i, position.z));
            }
        }
    }

    [ExecuteInEditMode]
    public void StartGeneratePool()
    {
        GeneratePool(StartTilesCount);
    }

    private void ClearMap(int sceneIndex)
    {
        foreach (var tile in _tilePool)
            ReturnToPool(tile);

        SceneManager.LoadScene(sceneIndex);
    }

    [ExecuteInEditMode]
    private void GeneratePool(int count)
    {
        _tilePool = new TileGeneration[count];
        Vector3 position = transform.position;

        for (int i = 0; i < TilesCount; i++)
        {
            _tilePool[i] = Instantiate(Tiles[Random.Range(0, Tiles.Count)]);
            _tilePool[i].SetPosition(new Vector3(position.x, position.y - i, position.z));
            _tilePool[i].GenerateDestroyObjects();
            _tilePool[i].GeneratenonDestroyObjects();
        }

    }
}
