using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapElementPool : MonoBehaviour
{
    [SerializeField] private List<MapElement> _nonDestroyObjects;
    [SerializeField] private List<MapElement> _destroyObjects;
    [SerializeField] private List<TileGeneration> _tiles;
    [SerializeField] private int _tilesCount;
    [SerializeField] private int _startTilesCount;

    public static MapElementPool Instance;

    private TileGeneration[] _tilePool;
    private ExitPanel _exitPanel;
    private DebugField _debugField;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
        Time.timeScale = 1;
        GeneratePool(_tilesCount);
    }

    public IMapElement GetNonDestroyObject(Transform parent)
    {
        IMapElement element = Instantiate(_nonDestroyObjects[Random.Range(0, _nonDestroyObjects.Count)], parent);
        element.RandomRotate();
        return element;
    }

    public IMapElement GetNonDestroyObject()
    {
        IMapElement element = _nonDestroyObjects[Random.Range(0, _nonDestroyObjects.Count)];
        element.RandomRotate();
        return element;
    }
    public IMapElement GetDestroyObject()
    {
        IMapElement element = _destroyObjects[Random.Range(0, _destroyObjects.Count)];
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
        if (_debugField == null)
        {
            _debugField = FindObjectOfType<DebugField>();
        }

        _debugField.ShowDebugText("in Get tile");

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

    private void ClearMap(int sceneIndex)
    {
        foreach (var tile in _tilePool)
            ReturnToPool(tile);

        SceneManager.LoadScene(sceneIndex);
    }

    private void GeneratePool(int count)
    {
        var tiles = FindObjectsOfType<TileGeneration>();

        if (tiles.Length > 0)
        {
            _tilePool = tiles;
        }
        else
        {
            _tilePool = new TileGeneration[count];
            Vector3 position = transform.position;

            for (int i = 0; i < _tilesCount; i++)
            {
                _tilePool[i] = Instantiate(_tiles[Random.Range(0, _tiles.Count)]);
                _tilePool[i].SetPosition(new Vector3(position.x, position.y - i, position.z));
                _tilePool[i].GenerateDestroyObjects();
                _tilePool[i].GeneratenonDestroyObjects();

                DontDestroyOnLoad((_tilePool[i] as TileGeneration).gameObject);
            }
        }
    }

    private IEnumerator GenerateTileByStep(Vector3 position)
    {
        for (int i = 0; i < _tilesCount; i++)
        {
            _tilePool[i] = Instantiate(_tiles[Random.Range(0, _tiles.Count)]);
            _tilePool[i].SetPosition(new Vector3(position.x, position.y - i, position.z));
            DontDestroyOnLoad((_tilePool[i] as TileGeneration).gameObject);
            yield return null;
        }
    }
}
