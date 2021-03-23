using UnityEngine;
public interface IItem
{
    Sprite ItemPicture { get; }
    string GetItemName { get; }
    string GetItemDescription { get; }
    CurrencyType CurrencyType { get; }
    ItemType ItemType { get; }
}

public enum ItemType
{
    AllTime,
    OneTime
}

