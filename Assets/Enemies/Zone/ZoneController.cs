using UnityEngine;

//класс для описания поведения зоны смерти, которая движется позади игрока
public class ZoneController : MonoBehaviour
{
    public float speed = 3f;
    public float offsetSpeed = 4f;
    public float accelerationPer = 1f;
    public GameObject[] parts;

    private int ActivePart;
    private int lenParts;
    private float walked = 0;
    private Vector3 vec = new Vector3();
    private Transform runner;

    public void Reset()
    {
        walked = 0;
        speed = 3f;
        offsetSpeed = 4f;
        accelerationPer = 1f;
        transform.position = new Vector3(0,0.1f,-120);
}
    void Start()
    {
        runner = FindObjectOfType<Player>().transform;
        ActivePart = 0;
        lenParts = parts.Length;
        parts[ActivePart].SetActive(true);

    }

    void FixedUpdate()
    {
        if (!GameController.Paused)
        {
            try
            {
                vec.x = runner.position.x - transform.position.x;
            }
            catch
            {
                vec.x = 0;
            }
            vec.z = speed * Time.deltaTime;
            gameObject.transform.position += vec;
            walked += vec.z;
            if (walked > 96)
            {
                walked = 0;
                speed *= (1 + accelerationPer / 100);
            }
            vec.x = offsetSpeed * Time.deltaTime;
            vec.z = 0;
            parts[ActivePart].transform.localPosition += vec;
            parts[(ActivePart + 1) % lenParts].transform.localPosition += vec;
            parts[(ActivePart + 2) % lenParts].transform.localPosition += vec;
            if (parts[(ActivePart + 2) % lenParts].transform.localPosition.x > 144)
            {
                parts[(ActivePart + 2) % lenParts].SetActive(false);
                ActivePart = ActivePart == 0 ? lenParts - 1 : --ActivePart;
                parts[ActivePart].SetActive(true);
                vec.x = parts[(ActivePart + 1) % lenParts].transform.localPosition.x - 96;
                parts[ActivePart].transform.localPosition = vec;
            }
        }
    }
}
