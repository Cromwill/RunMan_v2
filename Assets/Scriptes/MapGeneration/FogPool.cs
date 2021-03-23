using UnityEngine;
using System.Linq;

public class FogPool : MonoBehaviour
{
    [SerializeField] private Fog _fogPrefab;
    [SerializeField] private int _startCount;
    [SerializeField] private Vector3 _poolPosition;

    private Fog[] _fogPool;
    private ExitPanel _exit;

    private void Start()
    {
        _fogPool = null;
        GeneratePool(_startCount);
    }

    public void SetExitPanel(ExitPanel exit)
    {
        _exit = exit;
        _exit.OnExit += ClearFog;
    }

    public void ReturnToPool(Fog fog)
    {
        for (int i = 0; i < _fogPool.Length; i++)
        {
            if (_fogPool[i] == fog)
            {
                _fogPool[i].SetPosition(_poolPosition + Vector3.down * i);
                _fogPool[i].IsInThePool = true;
            }
        }
    }

    public Fog GetFog()
    {
        var fogs = _fogPool.Where(fog => fog.IsInThePool).ToArray();

        if (fogs.Length < 5)
             ExpandPool(_fogPool.Length + 10);

        return fogs.Where(fog => fog.IsInThePool).First();
    }

    private void ClearFog(int index)
    {
        foreach (var fog in _fogPool)
            fog.Clear();
    }

    private void GeneratePool(int count)
    {
        var fogs = FindObjectsOfType<Fog>();

        if (fogs.Length > 0)
            _fogPool = fogs;

        if (_fogPool == null)
        {
            _fogPool = new Fog[count];

            for (int i = 0; i < count; i++)
            {
                _fogPool[i] = CreateFog(i);
            }
        }
    }

    private void ExpandPool(int newSize)
    {
        Fog[] fogs = new Fog[newSize];

        for(int i = 0; i < fogs.Length; i++)
        {
            fogs[i] = _fogPool.Length - 1 < i ? CreateFog(i) : _fogPool[i]; 
        }

        _fogPool = fogs;
    }

    private Fog CreateFog(int index)
    {
        var fog = Instantiate(_fogPrefab);
        fog.SetPosition(_poolPosition + Vector3.down * index);
        DontDestroyOnLoad(fog.gameObject);
        return fog;
    }
}
