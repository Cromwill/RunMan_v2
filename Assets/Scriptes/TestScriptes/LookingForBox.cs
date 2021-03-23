using UnityEngine;

public class LookingForBox : MonoBehaviour
{
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    private Rigidbody _rigidBody;
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Turn();

        _rigidBody.velocity = transform.forward.normalized * _speed * Time.deltaTime;
    }

    private void Turn()
    {

        Vector3 direction = _target.transform.position - transform.position;
        float step = _turnSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction.normalized, step, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red, 2.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
