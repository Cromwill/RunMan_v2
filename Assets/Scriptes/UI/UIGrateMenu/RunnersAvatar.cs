using UnityEngine;

public class RunnersAvatar : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _isPresentation;

    private void Update()
    {
        if (_isPresentation)
            transform.Rotate(0, 1 * _speed * Time.deltaTime, 0, Space.World);
    }
}
