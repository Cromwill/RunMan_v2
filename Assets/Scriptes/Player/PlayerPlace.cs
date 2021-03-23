using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerPlace : MonoBehaviour
{
    [SerializeField] private PlayerPlaces _place;

    private GameObject[] _attachments;
    private bool _isUsed = false;

    private void Awake()
    {
        var elements = GetComponentsInChildren<SkinnedMeshRenderer>();
        _attachments = new GameObject[elements.Length];

        for(int i = 0; i < elements.Length; i++)
        {
            _attachments[i] = elements[i].gameObject;
        }
    }

    public void SetSkin(string skinName)
    {
        if (!_isUsed)
        {
            if (_attachments.Where(a => a.name == skinName).Count() != 0)
            {
                DisableAttachments(_attachments);
                GameObject onElement = _attachments.Where(a => a.name == skinName).First();
                onElement.SetActive(true);
                _isUsed = true;
            }
            else
            {
                DisableAttachments(_attachments);
            }
        }
    }

    public void UsedCompleat() => _isUsed = false;

    private void DisableAttachments(params GameObject[] attachments)
    {
        foreach(GameObject attachment in attachments)
        {
            attachment.SetActive(false);
        }
    }
}
