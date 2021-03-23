using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClouds : MonoBehaviour
{
    [SerializeField] private Sprite[] _cloudSprites;
    [SerializeField] private Vector3 _finishPosition;

    private Cloud[] _clouds;
    void Start()
    {
        _clouds = GetComponentsInChildren<Cloud>();

        foreach (var cloud in _clouds)
        {
            cloud.SetFinishPosition(_finishPosition);
            ChangeImage(cloud);
            cloud.SettedFinishPosition += ChangeImage;
        }
    }

    private void ChangeImage(Cloud cloud)
    {
        cloud.SetImage(_cloudSprites[Random.Range(0, _cloudSprites.Length)]);
    }

}
