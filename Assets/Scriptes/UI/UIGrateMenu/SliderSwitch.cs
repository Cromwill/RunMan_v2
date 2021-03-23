using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSwitch : MonoBehaviour
{
    private Slider _slider;

    public void SwitchSLider(int value)
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();

        int currentValue = (int)_slider.value + value;

        if (currentValue > _slider.maxValue || currentValue < _slider.minValue)
            return;
        else
            _slider.value = currentValue;
    }
}
