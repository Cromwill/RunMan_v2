using UnityEngine;

public class PlayerSkins : MonoBehaviour
{
    [SerializeField] private PlayerSkinData _skinData;

    private PlayerPlace[] _places;

    private void Start()
    {
        _places = GetComponentsInChildren<PlayerPlace>();
    }

    public void SetSkin(PlayerSkinData skinData)
    {
        var skins = skinData.GetSkins();

        if(_places == null)
            _places = GetComponentsInChildren<PlayerPlace>();

        foreach (var place in _places)
        {
            foreach (var skin in skins)
            {
                place.SetSkin(skin);
            }
        }

        foreach (var place in _places)
            place.UsedCompleat();
    }
}

public enum PlayerPlaces
{
    BackPack,
    Feet,
    Hair,
    Hands,
    HeadAttachments,
    Jacket,
    Legs,
    Torso,
    Vests
}
