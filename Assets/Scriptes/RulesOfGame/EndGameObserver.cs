using UnityEngine;

public class EndGameObserver : MonoBehaviour
{
    [SerializeField] private ExitPanel _exitPanel;

    private Player _player;
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _player.Deading += GameOver;
    }

    private void GameOver()
    {
        _exitPanel.ShowPanel(true, _player.scoreCounter.score, _player.scoreCounter.distance, _player.scoreCounter.money);
    }
}
