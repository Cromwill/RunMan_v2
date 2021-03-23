using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour
{
    [SerializeField] private Image _itemViewer;
    [SerializeField] private Text _itemName;
    [SerializeField] private Text _itemDescription;
    [SerializeField] private Image _currencyType;
    [SerializeField] private Text _price;

    [SerializeField] private Sprite[] _currencyTypes;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    [SerializeField] private PlayerScoreCounter _scoreCounter;
    [SerializeField] private Sprite[] _buyButtonImage;

    private IItem _currentItem;
    private Animator _selfAnimator;
    public void ShowPanel(IItem item)
    {
        gameObject.SetActive(true);
        if (_selfAnimator == null)
            _selfAnimator = GetComponent<Animator>();
        
        ShowItemInformation(item);    
        _currentItem = item;

        Sprite currentButtonSprite = SaveDataStorage.ItemContain(item) ? _buyButtonImage[1] : _buyButtonImage[0];
        _confirmButton.GetComponent<Image>().sprite = currentButtonSprite;
        SetConfirmButtonInteractable();
    }

    public void ClosePanel() => gameObject.SetActive(false);

    public void ProofOfPurchase()
    {
        Booster booster = _currentItem as Booster;
        Score price = GetItemScore(_currentItem);

        if (booster != null && booster.Type == BoosterType.Coin)
        {
            if(_scoreCounter.ReduceScore(price))
            {
                _scoreCounter.AddCoins((int)booster.Value);
                _selfAnimator.Play("Success");
            }
        }
        else if(booster != null && booster.CurrencyType == CurrencyType.Real)
        {
            _scoreCounter.BuyMoney(booster);
        }
        else if (_scoreCounter.ReduceScore(GetItemScore(_currentItem)))
        {
            booster.SaveBooster();
            _confirmButton.GetComponent<Image>().sprite = _buyButtonImage[1];
            _selfAnimator.Play("Success");
            SetConfirmButtonInteractable();
        }
    }

    private void ShowItemInformation(IItem item)
    {
        _itemViewer.sprite = item.ItemPicture;
        _itemName.text = item.GetItemName;
        _itemDescription.text = item.GetItemDescription;
        _currencyType.sprite = (item as Booster).CurrencyPicture;
        _price.text = (item as IBuyableObject).Price.ToString("0.##");
    }

    private Score GetItemScore(IItem item)
    {
        int price = (int)(item as IBuyableObject).Price;
        return item.CurrencyType == CurrencyType.Coin ? new Score(0, price) : new Score(price, 0);
    }

    private void SetConfirmButtonInteractable() => _confirmButton.interactable = !SaveDataStorage.ItemContain(_currentItem);
}
