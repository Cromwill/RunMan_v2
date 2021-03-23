using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelViewer : MonoBehaviour
{
    [SerializeField] private PlayerSkinData[] _skins;
    [SerializeField] private PlayerSkinData _defaultSkin;
    private ModelSwitcher _switcher;
    private PlayerSkins _avatar;
    private bool _isShop;
    private int[] _openedAvatars;
    private int _currentIdSkin;
    private Animator _selfAnimator;

    public IBuyableObject getCurrentAvatar => _skins.Where(a => a.Id == _currentIdSkin).FirstOrDefault();

    private void Start()
    {
        _avatar = GetComponentInChildren<PlayerSkins>();
        _selfAnimator = GetComponent<Animator>();
    }

    public void ToggleActiveObject(bool isShop, ModelSwitcher switcher)
    {
        _isShop = isShop;
        _switcher = switcher;

        if(!SaveDataStorage.HasKeyBuyableObjecty(_defaultSkin))
        {
            _currentIdSkin = _defaultSkin.Id;
            _avatar.SetSkin(_defaultSkin);
            SaveDataStorage.SaveBuyableObject(_defaultSkin);
            ToggleActiveObject(_isShop, _switcher);
        }
        else
        {
            _currentIdSkin = SaveDataStorage.LoadCurrentRunnersId();
            _openedAvatars = SaveDataStorage.LoadOpenedRunnersIds();
            _avatar.SetSkin(_skins.Where(a => a.Id == _currentIdSkin).First());
            ChangePrice();
        }
    }

    public void SetNextAvatar(int direction)
    {
        _selfAnimator.SetInteger("Direction", direction);
        _selfAnimator.Play("ChangeAvatar");
    }

    public void ChengeAvatar()
    {
        int newId = _currentIdSkin + _selfAnimator.GetInteger("Direction");

        if (_isShop)
        {
            if(newId > 0 && newId <= _skins.Length)
            {
                _currentIdSkin = newId;
                _avatar.SetSkin(_skins.Where(a=> a.Id == newId).First());
            }
        }
        else
        {
            if (_openedAvatars.Contains(newId))
            {
                _currentIdSkin = newId;
                _avatar.SetSkin(_skins.Where(a => a.Id == newId).First());
            }
        }
        
        ChangePrice();
    }

    public void ChangePrice()
    {
        _switcher.ChangePrice(_skins.Where(a => a.Id == _currentIdSkin).First().Price, _openedAvatars.Contains(_currentIdSkin));
    }
}
