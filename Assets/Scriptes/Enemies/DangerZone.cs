using UnityEngine;

public class DangerZone : MonoBehaviour
{
    private float radiuse;
    private ISearchablePlayers _spawner;

    private void Start()
    {
        _spawner = GetComponentInParent<ISearchablePlayers>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            _spawner.OnPlayerFounded(other.GetComponent<Player>());
    }
}
