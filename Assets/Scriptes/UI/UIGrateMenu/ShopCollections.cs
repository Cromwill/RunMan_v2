using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopCollections : PanelActivator
{
    [SerializeField] private Image[] _buttonImages;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private ConfirmPanel _confirmPanel;

    private ShopItem[] items;

    private void Start()
    {
        items = GetComponentsInChildren<ShopItem>();

        foreach(var item in items)
        {
            item.ItemChosen += BuyBooster;
        }
    }

    public void SelectedButton(Image currentImage)
    {
        currentImage.color = _selectedColor;

        foreach(var image in _buttonImages)
        {
            if (image != currentImage)
                image.color = Color.white;
        }
    }

    private void BuyBooster(IItem item)
    {
        _confirmPanel.ShowPanel(item);
    }
}
