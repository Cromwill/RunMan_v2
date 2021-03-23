using UnityEngine;
using UnityEngine.UI;

public class ModelSwitcher : MonoBehaviour
{
    [SerializeField] private ModelViewer _modelViewer;
    [SerializeField] private bool _isShop;
    [SerializeField] private Text _priceViewer;
    [SerializeField] private BuyPanel _buyPanel;

    private bool _isSold;
    private void OnEnable()
    {
        _modelViewer.ToggleActiveObject(_isShop, this);
        if (_isShop)
            _buyPanel.PurchaseСonfirmed += ChangePriceAfterPurChase;
    }

    private void OnDisable()
    {
        if (_isShop)
            _buyPanel.PurchaseСonfirmed -= _modelViewer.ChangePrice;

        _modelViewer.ToggleActiveObject(_isShop, this);
    }

    public void ChangePrice(float price, bool isSold = false)
    {
        _isSold = isSold;
        string priceText = _isSold ? "sold" : price.ToString("0.##") + "$";
        if (_isShop)
            _priceViewer.text = priceText;
    }

    public void ChangePriceAfterPurChase()
    {
        _modelViewer.ToggleActiveObject(_isShop, this);
    }

    public void BuyAvatar()
    {
        _buyPanel.OpenPanel(_modelViewer.getCurrentAvatar, _isSold);
    }

    public void ChooseRunners()
    {
        SaveDataStorage.SaveCurrentRunner(_modelViewer.getCurrentAvatar);
        Debug.Log("save runners");
    }

    public void NextAvatar() => _modelViewer.SetNextAvatar(1);
    public void PrevAvatar() => _modelViewer.SetNextAvatar(-1);
}
