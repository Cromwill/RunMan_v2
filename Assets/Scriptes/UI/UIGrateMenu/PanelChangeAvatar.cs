using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelChangeAvatar : MonoBehaviour
{
    [SerializeField] private ModelViewer _modelViewer;
    public void ToggleAvatar()
    {
        _modelViewer.ChengeAvatar();
    }
}
