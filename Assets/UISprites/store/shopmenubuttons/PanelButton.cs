using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//недописанный класс панели управления, стандартные кнопки не совсем отвечают требованиям, необходимо добавление логики
public class PanelButton : Selectable, ISelectHandler, IDeselectHandler
{
    public MaskableGraphic[] objects;
    public Color[] selectedObjectColor = { new Color(40 / 256f, 227 / 256f, 76 / 256f) };
    public Color[] deselectedObjectColor = { new Color(0 / 256f, 138 / 256f, 27 / 256f) };

    public override void OnSelect(BaseEventData data)
    {
        base.OnSelect(data);
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].color = selectedObjectColor[i];
        }
    }
    public override void OnDeselect(BaseEventData data)
    {
        base.OnDeselect(data);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].color = deselectedObjectColor[i];
        }
    }
}
