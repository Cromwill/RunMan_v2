using System;
using UnityEngine;

public class PlayerDamageZone : MonoBehaviour
{
    public event Action<Enemy> FindedEnemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            FindedEnemies?.Invoke(other.GetComponent<Enemy>());
        }
    }
}
