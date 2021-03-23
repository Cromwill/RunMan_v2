using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButtons : MonoBehaviour
{
    [SerializeField] private Scrollbar _selfScrollBar;

    private void Update()
    {

    }

    public void ShowPanel(int sliderValue)
    {
        gameObject.SetActive(true);
        _selfScrollBar.value = sliderValue;
    }
}
