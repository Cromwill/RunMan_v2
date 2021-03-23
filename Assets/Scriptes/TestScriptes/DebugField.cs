using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugField : MonoBehaviour
{
    private Text _text;
    private string _currentText = "";
    public GameObject _levelConstructor;
    public bool IsDebug;

    private void Awake()
    {
        ShowDebugText("start");
        ShowDebugText("LevelConstrGO - " + _levelConstructor.activeSelf);
        ShowDebugText("LevelConstrScript - " + _levelConstructor.GetComponent<LevelConstructor>());
    }

    public void ShowDebugText(string text)
    {
        if (IsDebug)
        {
            if (_text == null)
                _text = GetComponent<Text>();

            _currentText += "/_" + text + "_/";
            _text.text = _currentText;
        }
    }
}
