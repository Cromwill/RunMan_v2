using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChanging : MonoBehaviour
{
    [SerializeField] private float _changingSpeed;

    private Transform _selfTransform;

    private void Start()
    {
        _selfTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        _selfTransform.Rotate(new Vector3(0, _changingSpeed * Time.deltaTime, 0), Space.World);
    }
}
