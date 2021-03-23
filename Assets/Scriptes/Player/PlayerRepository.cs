using System.Linq;
using UnityEngine;

public class PlayerRepository : MonoBehaviour
{
    [SerializeField] private PlayerSkinData[] _skins;

    public PlayerSkinData GetCurrentSkin()
    {
        int id = SaveDataStorage.LoadCurrentRunnersId();
        if (id == 0) id = 1;

        return _skins.Where(a => a.Id == id).FirstOrDefault();
    }

    public void GetSkinForId(int id)
    {

    }
}
