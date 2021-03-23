using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Booster _booster;
    [SerializeField] private Image _productShower;
    [SerializeField] private Image _currensyShower;
    [SerializeField] private Text _counter;

    private Button _buyButton;
    private Text _priceViewer;

    public event Action<IItem> ItemChosen;

    private void OnEnable()
    {
        if (_priceViewer == null)
            _priceViewer = GetComponentInChildren<Text>();
        if (_buyButton == null)
            _buyButton = GetComponentInChildren<Button>();
        if (_counter != null)
            _counter.text = _booster.Value.ToString("0.##");
        if (_booster.ItemPicture != null)
            _productShower.sprite = _booster.ItemPicture;

        _buyButton.onClick.AddListener(delegate { ItemChosen?.Invoke(_booster); });
        _currensyShower.sprite = _booster.CurrencyPicture;

        if (!SaveDataStorage.ItemContain(_booster))
        {
            string format = _booster.CurrencyType == CurrencyType.Real ? "0.00#" : "0.##";
            _priceViewer.text = _booster.GetItemPrice.ToString(format);
        }
        else
            _priceViewer.text = "SOLD";
    }
}
