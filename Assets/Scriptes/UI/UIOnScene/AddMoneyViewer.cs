using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class AddMoneyViewer : MonoBehaviour
{
    [SerializeField] private float _delayTimeStart;
    [SerializeField] private string _startAddingAnimationName;
    [SerializeField] private string _showMoneyValueAnimationName;

    private Text _moneyViewer;
    private Animator _selfAnimator;
    private int _money;

    public event Action ConvertToMoneyStarting;

    private void Start()
    {
        _moneyViewer = GetComponentInChildren<Text>();
        _selfAnimator = GetComponent<Animator>();
    }

    public void ShowAddingMoney() => StartCoroutine(StartAddingAnimation(_delayTimeStart));

    public void ContinueAnimation() => ConvertToMoneyStarting?.Invoke();

    public void ShowAddingMoneyAnimation(int money)
    {
        _money = money;
        _selfAnimator.Play(_showMoneyValueAnimationName);
    }

    public void ShowMoney()
    {
        _moneyViewer.text = _money.ToString("0.##");
    }

    private IEnumerator StartAddingAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        _selfAnimator.Play(_startAddingAnimationName);
    }
}
