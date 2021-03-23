using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FogConstructor))]
public class LevelConstructor : MonoBehaviour
{
    [SerializeField] private int _verticalRange;
    [SerializeField] private int _horizontalRange;
    [SerializeField] private MapElementPool _pool;
    [SerializeField] private TileGeneration _startTile;

    private List<TileGeneration> _currentTiles;
    private bool _isFinishGenerateTiles = false;
    private FogConstructor _fogConstructor;
    private EnemiesConstructor _enemiesConstructor;
    private DebugField _debugField;

    private void OnDestroy()
    {
        _currentTiles.Clear();
    }

    private void Awake()
    {
        _debugField = FindObjectOfType<DebugField>();
        _debugField.ShowDebugText("awake in level Constructor");
        //Time.timeScale = 1;
        //GenerateLevel(_startTile);
        //_currentTiles = new List<ITile>();
        //_currentTiles.Add(_startTile);
        //_pool = FindObjectOfType<MapElementPool>();
        //_fogConstructor = GetComponent<FogConstructor>();
        //_enemiesConstructor = GetComponent<EnemiesConstructor>();
    }

    private void Start()
    {
        Time.timeScale = 1;
        //_startTile.CheckPosition += GenerateLevel;
        
        _currentTiles = new List<TileGeneration>();
        _currentTiles.Add(_startTile);
        _pool = FindObjectOfType<MapElementPool>();
        _fogConstructor = GetComponent<FogConstructor>();
        _enemiesConstructor = GetComponent<EnemiesConstructor>();
        GenerateLevel(_startTile);

        _debugField.ShowDebugText("start in level Constructor");
    }

    public List<TileGeneration> GetZLine(float zPosition) => _currentTiles.Where(t => t.GetPosition().z == zPosition).ToList();

    public List<TileGeneration> GetTiles() => _currentTiles;

    private void GenerateLevel(TileGeneration currentTile)
    {
        float tileXSize = 10;
        float tileZSize = 10;

        List<Vector3> requiredCoordinates = new List<Vector3>();

        for (int i = _horizontalRange * -1; i <= _horizontalRange; i++)
        {
            for (int j = (_verticalRange / 2) * -1; j <= _verticalRange; j++)
            {
                float xPosition = currentTile.GetPosition().x + i * tileXSize;
                float zPosition = currentTile.GetPosition().z + j * tileZSize;
                requiredCoordinates.Add(new Vector3(xPosition, 0, zPosition));
            }
        }

        foreach (var coordinate in requiredCoordinates)
        {
            if (_currentTiles.Where(tile => tile.GetPosition() == coordinate).ToArray().Count() == 0)
                _currentTiles.Add(GenerateTile(coordinate));
        }

        if (!_isFinishGenerateTiles)
        {
            _isFinishGenerateTiles = true;
            _fogConstructor.StartGenerate(this, _startTile);
        }

        _enemiesConstructor.GenerateEnemeSpawners(_currentTiles.ToArray(), currentTile);
    }

    private void ExitLevel()
    {
        foreach(var tile in _currentTiles)
        {
            if (!tile.IsHaveFog)
                _pool.ReturnToPool(tile);
        }
    }

    private TileGeneration GenerateTile(Vector3 position, bool isFirstTile = false)
    {
        TileGeneration tile;

        if (_pool.IsPositionEmpty(position))
        {
            tile = _pool.GetTile();
            tile.ReturningToPool += delegate (TileGeneration currentTile)
            {
                _currentTiles.Remove(currentTile);
            };
            tile.SetPosition(position);
            tile.CheckPosition += GenerateLevel;
        }
        else
        {
            tile = _pool.GetTile(position);
            tile.CheckPosition += GenerateLevel;
            tile.ReturningToPool += delegate (TileGeneration currentTile)
            {
                _currentTiles.Remove(currentTile);
            };
        }
        return tile;
    }

    private void SubscribeOnTileAction(TileGeneration tile)
    {
        tile.ReturningToPool += delegate (TileGeneration currentTile)
        {
            _currentTiles.Remove(currentTile);
        };
        tile.CheckPosition += GenerateLevel;
    }
}
