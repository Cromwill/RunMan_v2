using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Image _image;
    private Vector3 _startPosition;
    private Vector3 _finishPosition;

    public event Action<Cloud> SettedFinishPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _startPosition.x = -5;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if(transform.position.x > _finishPosition.x)
        {
            transform.position = _startPosition;
            SettedFinishPosition?.Invoke(this);
        }
    }

    public void SetImage(Sprite sprite)
    {
        if (_image == null)
            _image = GetComponent<Image>();
        _image.sprite = sprite;
    }

    public void SetFinishPosition(Vector3 position) => _finishPosition = position;
}

public enum CloudDirectionMovement
{
    Left = -1,
    Right = 1
}
