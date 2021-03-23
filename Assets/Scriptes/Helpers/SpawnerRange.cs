using System;
using UnityEngine;
public class SpawnerRange
{
    private int _stepX;
    private int _stepZ;

    private Vector3 _minPosition;
    private Vector3 _maxPosition;

    public TileGeneration getTile { get; }
    public SpawnerRange(TileGeneration tile, int stepX, int stepZ)
    {
        getTile = tile;
        _stepX = stepX;
        _stepZ = stepZ;
        FillingRange();
    }

    public bool IsInTheRange(Vector3 position)
    {
        return position.x < _maxPosition.x && position.z < _maxPosition.z &&
            position.x > _minPosition.x && position.z >= _minPosition.z;
    }

    private void FillingRange()
    {
        Vector3 center = getTile.GetPosition();
        _minPosition = new Vector3(center.x - _stepX * getTile.GetSize().x, center.y, center.z - _stepZ * getTile.GetSize().z);
        _maxPosition = new Vector3(center.x + _stepX * getTile.GetSize().x, center.y, center.z + _stepZ * getTile.GetSize().z);
    }


}

