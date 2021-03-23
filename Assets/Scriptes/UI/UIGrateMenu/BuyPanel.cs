using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyPanel : MonoBehaviour
{
    [SerializeField] private PlayerScoreCounter _scoreCounter;
    [SerializeField] private Text _panelMessage;
    [SerializeField] private Button _acceptButton;

    [SerializeField] private string _buyMessage;
    [SerializeField] private string _soldMessage;

    public event Action PurchaseСonfirmed;

    private Animator _selfAnimator;
    private IBuyableObject _buyableObject;
    public void OpenPanel(IBuyableObject buyableObject, bool isSold)
    {
        gameObject.SetActive(true);
        _buyableObject = buyableObject;
        _panelMessage.text = isSold ? _soldMessage : _buyMessage;
        _acceptButton.interactable = !isSold;
    }

    public void Accept()
    {
        if (_scoreCounter.ReduceScore(new Score((int)_buyableObject.Price, 0)))
        {
            _scoreCounter.SaveBuyableObject(_buyableObject);
            PurchaseСonfirmed?.Invoke();
            Cancel();
        }
        else
        {
            if (_selfAnimator == null)
                _selfAnimator = GetComponent<Animator>();
            _selfAnimator.Play("Cancel");
        }
    }

    public void Cancel()
    {
        _buyableObject = null;
        gameObject.SetActive(false);
    }
}
