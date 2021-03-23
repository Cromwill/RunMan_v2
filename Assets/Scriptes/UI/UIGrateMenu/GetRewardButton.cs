using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class GetRewardButton : MonoBehaviour
{
    private Animator _selfAnimator;
    private Button _selfButton;

    public event Action Collected;

    public void Show()
    {
        if (_selfAnimator == null)
        {
            _selfAnimator = GetComponent<Animator>();
            _selfButton = GetComponent<Button>();
        }

        _selfButton.onClick.AddListener(Collect);
    }

    public void OnCollected()
    {
        Debug.Log("OnCollected");
        gameObject.SetActive(false);
        Collected?.Invoke();
    }

    private void Collect()
    {
        _selfAnimator.Play("RewardedButtonCollect");
        _selfButton.onClick.RemoveListener(Collect);
    }
}
