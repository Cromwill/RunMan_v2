using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreViewer : MonoBehaviour
{
    [SerializeField] private Text _moneyViewer;
    [SerializeField] private Text _coinsViewer;

    public void ShowScore(Score score)
    {
        _moneyViewer.text = score.Money.ToString("0.##");
        _coinsViewer.text = score.Coins.ToString("0.##");
    }
}
