using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementShower : MonoBehaviour
{
    [SerializeField] private Achievement _achievement;
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _progressShower;
    [SerializeField] private Text _achievementName;
    [SerializeField] private GetRewardButton _getRewardButton;

    private void OnEnable()
    {
        Debug.Log("Shower on Enable");
        _getRewardButton.Collected += CollectRewarded;
        if (_slider != null && _achievement != null)
            Show();
    }

    private void OnDisable()
    {
        _getRewardButton.Collected -= CollectRewarded;
    }

    private void Show()
    {
        int value = (int)_achievement.GetCurrentValue();
        _achievementName.text = _achievement.ShowName.ToUpper();

        for (int i = 0; i < _achievement.Condition.Length; i++)
        {
            if (value < _achievement.Condition[i])
            {
                _progressShower.text = value + " / " + (int)_achievement.Condition[i];
                _slider.value = 1 / _achievement.Condition[i] * value;
                return;
            }

            if(!_achievement.IsRewardCollection(i))
            {
                ShowRewardedButton();
                return;
            }
        }
    }

    private void ShowRewardedButton()
    {
        _slider.gameObject.SetActive(false);
        _progressShower.gameObject.SetActive(false);
        _getRewardButton.gameObject.SetActive(true);
        _getRewardButton.Show();
    }

    private void CollectRewarded()
    {
        _slider.gameObject.SetActive(true);
        _progressShower.gameObject.SetActive(true);
        FindObjectOfType<PlayerScoreCounter>().AddMoney(_achievement.GetReward());
        Show();
    }
}
