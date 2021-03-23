using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    [SerializeField] private Text _totalText;
    [SerializeField] private SkillPoints[] _skills;
    [SerializeField] private PlayerScoreCounter _scoreCounter;

    public void ShowTotal()
    {
        _totalText.text = GetTotalValue().ToString("0.##");
    }
    public void ConfirmSkills()
    {
        Score price = new Score(0, GetTotalValue());

        if (_scoreCounter.IsCanBuy(price))
        {
            _scoreCounter.ReduceScore(price);
            foreach (var skill in _skills)
            {
                skill.DataSave();
            }
            ShowTotal();
        }
    }

    public void CancelScills()
    {
        foreach(var skill in _skills)
        {
            skill.DataReset();
        }

        ShowTotal();
    }

    private int GetTotalValue()
    {
        int value = 0;

        foreach (var skill in _skills)
        {
            value += skill.Coast;
        }

        return value;
    }
}
