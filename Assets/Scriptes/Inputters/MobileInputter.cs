using UnityEngine;

public class MobileInputter : MonoBehaviour, IInputter
{
    [SerializeField] private Player _player;

    private bool _isPlayerDead;

    private void Start()
    {
        _player.Deading += delegate { _isPlayerDead = true; };
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && !_isPlayerDead)
        {
            PlayerTurn(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
    }

    private void PlayerTurn(Vector3 ScreenToViewportPoint)
    {
        RotateDirection direction = ScreenToViewportPoint.x < 0.5f ? RotateDirection.Left : RotateDirection.Right;
        _player.Turn(direction);
    }
}
