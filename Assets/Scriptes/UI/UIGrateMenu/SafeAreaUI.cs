using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaUI : MonoBehaviour
{
    private void Awake()
    {
        var safeArea = Screen.safeArea;
    }
}
