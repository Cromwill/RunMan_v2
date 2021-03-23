using System.Collections;
using UnityEngine;

// класс отвечающий за управление чанками: спавн объектов на них, включение и выключении опциональных частей, поворот наполнения и прочее
public class Chunk : MonoBehaviour
{
    
    public GameObject[] optional;
    public ChunkBase points;
    MapGenerator mapGenerator;
    int hash;
    

    void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        StartCoroutine(SpawnObjects());
    }

    public void Move(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    public void SetHash(int h)
    {
        hash = h;
        //Debug.Log(hash);
    }

    public IEnumerator Expand()
    {
        yield return StartCoroutine(mapGenerator.GenerateMap(hash));
        yield return SpawnObjects();
    }
    
    public IEnumerator SpawnObjects()
    {
        for (int i = 0; i < points.spawnPoints.Length; i++)
        {
            int j = GameController.RandomDrop();
            if (j != -1)
            {
                GameObject obj = Instantiate(GameController.gameController.spawnableObjects[j], points.spawnPoints[i]);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}

