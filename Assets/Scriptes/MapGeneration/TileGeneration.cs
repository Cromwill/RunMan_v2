using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileGeneration : MonoBehaviour
{
    public Vector3[] NotDestroyObjectPositions;
    public Vector3[] DestroyPositions;
    public Vector3 EnemiesSpawnDot;

    private Mesh _mesh;
    private MapElementPool _pool;
    private EnemiesSpawner _spawner;
    private List<IMapElement> _mapElements = new List<IMapElement>();

    public bool IsInThePool { get; set; } = true;
    public bool IsHaveSpawner => _spawner != null;
    public bool IsHaveFog { get; private set; }
    public event Action<TileGeneration> CheckPosition;
    public event Action<TileGeneration> ReturningToPool;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;

        if (NotDestroyObjectPositions != null)
        {
            for (int i = 0; i < NotDestroyObjectPositions.Length; i++)
                Gizmos.DrawCube(transform.position + NotDestroyObjectPositions[i], new Vector3(1, 1, 0.5f));
        }

        if (DestroyPositions != null)
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < DestroyPositions.Length; i++)
                Gizmos.DrawSphere(transform.position + DestroyPositions[i], 0.35f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + EnemiesSpawnDot, 0.35f);
    }

    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;

        CheckPosition = null;
        ReturningToPool = null;
        IsHaveFog = false;
    }

    public void GenerateDestroyObjects() => GenerateTile(DestroyPositions, MapElementTypes.Destroy);
    public void GeneratenonDestroyObjects() => GenerateTile(NotDestroyObjectPositions, MapElementTypes.NotDestroy);

    public void SetPosition(Vector3 position) => transform.position = position;

    public Vector3 GetPosition() => transform.position;

    public Vector3 GetSize()
    {
        if(_mesh == null)
            _mesh = GetComponent<MeshFilter>().mesh;

        return _mesh.bounds.size;
    }

    public void AddFog(Fog fog)
    {
        IsHaveFog = true;
        fog.Destriction += ReturnToPool;
    }

    public void AddSpawner(EnemiesSpawner spawner)
    {
        _spawner = spawner;
        _spawner.DestringSpawner += RemoveSpawner;
    }

    private void RemoveSpawner() => _spawner = null;

    private void ReturnToPool(Fog fog)
    {
        fog.Destriction -= ReturnToPool;
        IsHaveFog = false;
        ReturningToPool?.Invoke(this);

        if (_spawner != null)
            Destroy(_spawner.gameObject);
        _pool.ReturnToPool(this);
        ReturningToPool = null;
    }

    public void ReturnToPool(Vector3 poolPosition)
    {
        IsHaveFog = false;
        IsInThePool = true;
        ReturningToPool?.Invoke(this);
        ReturningToPool = null;
        if (_spawner != null)
            Destroy(_spawner.gameObject);

        SetPosition(poolPosition);
    }


    private void GenerateTile(Vector3[] positions, MapElementTypes mapElementTypes)
    {

        if (_pool == null)
            _pool = FindObjectOfType<MapElementPool>();

        for (int i = 0; i < positions.Length; i++)
        {
            IMapElement prefab = mapElementTypes == MapElementTypes.Destroy ? _pool.GetDestroyObject() : _pool.GetNonDestroyObject();
            IMapElement mapElement = Instantiate((MapElement)prefab, transform);
            mapElement.SetElement(GetPosition() + positions[i]);
            _mapElements.Add(mapElement);
            DontDestroyOnLoad((mapElement as MapElement).gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
            CheckPosition?.Invoke(this);
    }


}

public enum MapElementTypes
{
    Destroy,
    NotDestroy
}
