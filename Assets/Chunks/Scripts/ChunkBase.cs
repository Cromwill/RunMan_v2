using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//основа генератора, именно этот класс отвечает за начало построения чанков вокруг в течение игры
public class ChunkBase : MonoBehaviour
{
    public Transform[] binds;
    public Transform[] spawnPoints;
    
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            StartCoroutine(GetComponentInParent<Chunk>().Expand());
        }
    }
}
