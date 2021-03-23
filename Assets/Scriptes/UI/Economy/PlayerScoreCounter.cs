using UnityEngine;

[RequireComponent(typeof(InAppManager))]
public class PlayerScoreCounter : MonoBehaviour
{
    [SerializeField] private PlayerScoreViewer _scoreViewer;

    private Score _currentScore;
    private InAppManager _iAPMeneger;

    private void Awake()
    {
        _currentScore = SaveDataStorage.LoadScore();
        _scoreViewer.ShowScore(_currentScore);
        _iAPMeneger = GetComponent<InAppManager>();
    }

    public bool ReduceScore(Score score)
    {
        if (score <= _currentScore)
        {
            _currentScore -= score;
            SaveDataStorage.SaveScore(_currentScore);
            _scoreViewer.ShowScore(_currentScore);
            return true;
        }
        else
            return false;
    }

    public void SaveBuyableObject(IBuyableObject buyable)
    {
        SaveDataStorage.SaveBuyableObject(buyable);
    }

    public bool IsCanBuy(Score price) => _currentScore >= price;

    public void AddMoney(int value)
    {
        _currentScore += new Score(value, 0);
        ReduceScore(new Score(0, 0));
    }

    public void AddCoins(int value)
    {
        _currentScore += new Score(0, value);
        ReduceScore(new Score(0, 0));
    }

    public void BuyMoney(Booster booster)
    {
        _iAPMeneger.BuyProductID(booster.ItemIOSId);
    }

    public void Clean()
    {
        PlayerPrefs.DeleteAll();
    }
}
