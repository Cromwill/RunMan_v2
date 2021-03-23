using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GrateButtons : MonoBehaviour
{
    [SerializeField] private TabButtons _tabButtons;
    [SerializeField] private Image[] _buttonImages;

    [SerializeField] private Image _leftImages;
    [SerializeField] private Image _rightImages;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Scrollbar _horizontalScrollbar;

    private AsyncOperation _gameScenePlay;

    private void Awake()
    {
        _horizontalScrollbar.onValueChanged.AddListener(ChangeScrollbarValue);
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowGarage()
    {
        _tabButtons.ShowPanel(1);
    }

    public void ShowShop()
    {
        _tabButtons.ShowPanel(0);
    }

    private void SelectedButton(Image currentImage)
    {
        currentImage.color = _selectedColor;

        foreach (var image in _buttonImages)
        {
            if (image != currentImage)
                image.color = Color.white;
        }
    }

    private void ChangeScrollbarValue(float value)
    {
        if (_horizontalScrollbar.value < 0.35)
            SelectedButton(_leftImages);
        else if(_horizontalScrollbar.value > 0.65)
            SelectedButton(_rightImages);
        else
        {
            _leftImages.color = Color.white;
            _rightImages.color = Color.white;
        }    
    }
}
