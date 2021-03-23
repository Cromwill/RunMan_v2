using UnityEngine;

//класс отвечающий за логику всей игры, выносится в пустой объект, который неподвижен на сцене, к нему цепляем mapGenerator
public class GameController : MonoBehaviour
{
    public GameObject[] map;
    GameObject player;
    public GameObject[] spawnableObjects;
    public static GameController gameController;

    public static bool Paused;
    public static int score;
    public static int distance;

    private int[] distScore;

    public void Reset()
    {
        for (int i = 0; i < 1024; i++)
        {
            Destroy(map[i]);
        }
        map = new GameObject[1024];
        distScore = new int[8];
        score = 0;
        distance = 0;
    }

    void Awake()
    {
        gameController = this;
        map = new GameObject[256];
        score = 0;
        distScore = new int[8];
        player = FindObjectOfType<Player>().gameObject;
    }

    void LateUpdate()
    {
        int dist = Mathf.RoundToInt(player.transform.position.magnitude);
        if (distance < dist)
        {
            score += dist - distance;
            distance = dist;
        }

        dist = distance / 100;
        if (dist > distScore[0])
        {
            score += 5 * (dist - distScore[0]);
            distScore[0] = dist;
        }

        dist = distance / 500;
        if (dist > distScore[1])
        {
            score += 35 * (dist - distScore[1]);
            distScore[1] = dist;
        }

        dist = distance / 1000;
        if (dist > distScore[3])
        {
            score += 100 * (dist - distScore[3]);
            distScore[3] = dist;
        }
        dist = distance / 5000;
        if (dist > distScore[4])
        {
            score += 750 * (dist - distScore[4]);
            distScore[4] = dist;
        }
    }

    public static int RandomDrop()
    {
        int i = Random.Range(0, 100);
        if (i > 10 && i < 20)
        {
            return 0;
        }
        if (i > 20 && i < 30)
        {
            return 1;
        }
        return -1;
    }
}
