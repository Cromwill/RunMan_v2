using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarStepper : MonoBehaviour
{
    [SerializeField] private float[] _steps;
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private float _time;

    [SerializeField] private bool _isAppForMobile;

    private bool _isStartScreenMoved;

    private float _currentPositionValue;
    private float _finishPosition;

    private void Start()
    {
        _scrollbar.onValueChanged.AddListener(OnChangeValue);
        _currentPositionValue = _scrollbar.value;
    }

    private void Update()
    {
        if (_isStartScreenMoved)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _isStartScreenMoved = false;
                StartValueChanging();
            }
        }
    }

    private void OnChangeValue(float value) => _isStartScreenMoved = true;

    private void StartValueChanging()
    {
        ScrollDirection direction = _currentPositionValue > _scrollbar.value ? ScrollDirection.Left : ScrollDirection.Right;
        if (_scrollbar.value < 0 || _scrollbar.value > 1)
        {
            bool isValueLessThanZero = _scrollbar.value < 0;
            _finishPosition = isValueLessThanZero ? 0 : 1;
            direction = isValueLessThanZero ? ScrollDirection.Right : ScrollDirection.Left;
        }
        else
        {
            for (int i = 0; i < _steps.Length - 1; i++)
            {
                if (IsValueIsInRange(_scrollbar.value, _steps[i], _steps[i + 1]))
                {
                    _finishPosition = direction == ScrollDirection.Left ? _steps[i] : _steps[i + 1];
                }
            }
        }
        float changingSpeed = Mathf.Abs(_finishPosition - _scrollbar.value) / _time;

        StartCoroutine(ScrollbarValueChanging(direction, changingSpeed));
    }

    private IEnumerator ScrollbarValueChanging(ScrollDirection direction, float speed)
    {
        while (IsValueChanging(direction))
        {
            _scrollbar.value += (speed * Time.deltaTime) * (int)direction;
            yield return null;
        }

        _scrollbar.value = _finishPosition;
        _currentPositionValue = _finishPosition;
    }

    private bool IsValueChanging(ScrollDirection direction) => _finishPosition < _currentPositionValue ?
        _scrollbar.value > _finishPosition : _scrollbar.value < _finishPosition;

    private bool IsValueIsInRange(float value, float min, float max) => value >= min && value <= max;

    private enum ScrollDirection
    {
        Left = -1,
        Right = 1
    }

    private bool GetPointerOverGameObject()
    {
        return _isAppForMobile ? !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) : !EventSystem.current.IsPointerOverGameObject();
    }

}
