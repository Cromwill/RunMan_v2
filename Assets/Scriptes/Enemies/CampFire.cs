using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public GameObject zombie_Prefab;
    public int countZombie = 2;
    void Start()
    {
        Vector3 spawnPoint = new Vector3(0,1,0);
        while (countZombie > 0)
        {
            spawnPoint.x = Random.Range(-1.5f, 1.5f);
            spawnPoint.z = Random.Range(-1.5f, 1.5f);
            if (!Physics.Linecast(transform.position + Vector3.up, spawnPoint))
            {
                Instantiate(zombie_Prefab, transform.position + spawnPoint, new Quaternion());
                countZombie--;
            }
        }
    }
}
