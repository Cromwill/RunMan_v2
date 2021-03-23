using UnityEngine;

[CreateAssetMenu(fileName = "new Booster", menuName = "Booster SO")]
public class Booster : ScriptableObject, IItem, IBuyableObject
{
    [SerializeField] private BoosterType _boosterType;
    [SerializeField] private float _changingValue;
    [SerializeField] private float _price;
    [Multiline]
    [SerializeField] private string _descriptions;
    [SerializeField] private string _itemName;
    [SerializeField] private CurrencyType _currencyType;
    [SerializeField] private Sprite _itemPicture;
    [SerializeField] private Sprite _currencyPicture;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private string _iOSShopIds;

    public BoosterType Type => _boosterType;
    public float Value => _changingValue;
    public float GetItemPrice => _price;
    public Sprite ItemPicture => _itemPicture;
    public Sprite CurrencyPicture => _currencyPicture;
    public string GetItemName => _itemName;
    public string GetItemDescription => _descriptions;
    public CurrencyType CurrencyType => _currencyType;
    public ItemType ItemType => _itemType;
    public float Price => _price;
    public int Id => 0;
    public string ItemIOSId => _iOSShopIds;

    string IBuyableObject.Type => _boosterType.ToString();

    public void SaveBooster()
    {
        SaveDataStorage.SaveBuyableObject(this);
    }

    public void DeleteBooster()
    {
        SaveDataStorage.SaveBuyableObject(this, false);
    }
}

public enum BoosterType
{
    Speed,
    Damage,
    Item,
    Score,
    Life,
    Coin,
    Money
}

public enum CurrencyType
{
    Money = 0,
    Coin = 1,
    Comertial,
    Real
}
