using UnityEngine;
//класс-костыль для программирования движения отедльных кусочков зоны разрушения
public class Block : MonoBehaviour
{
    private float speed;
    public float acceleration = 0.5f;

    void Start()
    {
        speed = (-Random.Range(0.1f, 1f));
    }
    void FixedUpdate()
    {
        
        speed += acceleration * Time.deltaTime;

        transform.Translate(0, 0, speed);

        if (speed > 1f)
        {
            Init();
        }
    }

    void Init()
    {
        speed = (-Random.Range(0.1f, 0.5f));
        Vector3 pos = transform.localPosition;
        pos.z = 1.45f;
        transform.localPosition = pos;
    }
}
