using System.Collections;
using UnityEngine;

//генератор строит вокруг заданного чанка ещё 8 и хранит их в массиве под номерами соответствующими их хэшу
//в силу изоморфности пространства матриц 32*32 и списков длиной 1024 можно однозначно определять адреса
//порядок генерации чанков прописан ниже, его важно соблюдать при расставлении точек, в идеале просто не трогать chunkBase во всех префабах и копировать его при создании новых
public class MapGenerator : MonoBehaviour
{
    public GameObject[] ChunkPrefabs;
    GameController gameController;
    int lastChunk = 0;
    public static bool inProgress = false;
    int[] activeChunk = new int[9];
    public void Reset()
    {
        gameController = GetComponent<GameController>();
        int hash_init = 528;
        int i = Random.Range(0, ChunkPrefabs.Length);
        gameController.map[hash_init] = Instantiate(ChunkPrefabs[i]);
        Chunk NewChunk = gameController.map[hash_init].GetComponentInChildren<Chunk>();
        NewChunk.SetHash(hash_init);
        NewChunk.Move(new Vector3(0, 0, 0));
    }

    void Start()
    {
        gameController = GetComponent<GameController>();
        int hash_init = 128;
        for(int j = 0; j < 9; j++)
        {
            activeChunk[j] = hash_init;
        }
        int i = Random.Range(0, ChunkPrefabs.Length);
        gameController.map[hash_init] = Instantiate(ChunkPrefabs[i]);
        Chunk NewChunk = gameController.map[hash_init].GetComponentInChildren<Chunk>();
        NewChunk.SetHash(hash_init);
        NewChunk.Move(new Vector3(0,0,0));
    }

    //Генератор окружающих чанков
    /* 7 6 5 *
     * 4 - 3 *
     * 2 1 0 */
    public IEnumerator GenerateMap(int hash)
    {
        int[] active = new int[9];
        int hash_n;
        active[4] = hash;
        if (lastChunk == hash) yield break;
        lastChunk = hash;
        Debug.Log(hash);
        Transform[] pos = gameController.map[hash].GetComponent<Chunk>().points.binds;
        Vector3 start = gameController.map[hash].transform.position;
        inProgress = true;
        int x = hash / 16;
        int y = hash % 16;
        for (int i = -1; i <= 1; i++)
        {
            x += i;
            if (x > 15) x = 0;
            if (x < 0) x = 15; 
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                y += j;
                if (y < 0) y = 15;
                if (y > 15) y = 0;
                hash_n = 16 * x + y;
                //Если объект существует, активировать
                int k = i + j < 1 && i < 1 ? (i + 1) * 3 + j + 1 : (i + 1) * 3 + j;
                if (k < 4) active[k] = hash_n;
                else active[k + 1] = hash_n;
                if (gameController.map[hash_n] != null)
                {
                    gameController.map[hash_n].SetActive(true);
                    yield return new WaitForFixedUpdate();
                    Debug.Log("P: " + hash + " H: " + hash_n + " x: " + x + " y: " + y + " K: " + k + " A" + " i: " + i + " j: " + j);
                    y = hash % 16;
                    continue;
                }
                //Если не существует, заспавнить в соответствии с картой поверхности
                int m = Random.Range(0, ChunkPrefabs.Length);
                Debug.Log("P: " + hash + " H: " + hash_n + " x: " + x + " y: " + y + " K: " + k + " I" + " i: " + i + " j: " + j);
                gameController.map[hash_n] = Instantiate(ChunkPrefabs[m], 2 * pos[k].localPosition + start, new Quaternion());
                Chunk newChunk = gameController.map[hash_n].GetComponent<Chunk>();
                newChunk.SetHash(hash_n);
                //NewChunk.StartCoroutine(NewChunk.SpawnObjects());
                yield return new WaitForFixedUpdate();
                y = hash % 16;


            }
            x = hash / 16;
        }    
        foreach(int i in activeChunk)
        {
            bool o = false;
            foreach(int j in active)
            {
                if (i == j)
                {
                    o = true;
                    break;
                }
            }
            if (o) continue;
            gameController.map[i].SetActive(false);
        }
        activeChunk = active;
        inProgress = false;
    }
}
