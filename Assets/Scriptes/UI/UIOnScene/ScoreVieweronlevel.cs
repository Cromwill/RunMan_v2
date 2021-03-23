using UnityEngine;
using UnityEngine.UI;

public class ScoreVieweronlevel : MonoBehaviour
{
    [SerializeField] private Text _scoreValue;
    [SerializeField] private Text _distanceValue;

    public void ShowDistance(float value) => _distanceValue.text = value.ToString("0.0#");
    public void ShowScore(int value)
    {
        _scoreValue.text = value.ToString();
    }
}
