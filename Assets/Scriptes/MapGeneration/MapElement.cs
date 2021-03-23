using UnityEngine;
using System.Collections;

public class MapElement : MonoBehaviour, IMapElement
{
    public void SetElement(Vector3 position) => transform.position = position;

    public void RandomRotate()
    {
        transform.rotation = Quaternion.Euler(Random.Range(-10, 11), Random.Range(0, 360), Random.Range(-10, 11));
    }

}
