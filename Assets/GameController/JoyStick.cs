using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//класс джойстика, просто игроку указывается объект с которого читать данные, может и с него самого и дальше просто вызываются нужные методы
public class JoyStick : MonoBehaviour
{
    public int maxLength;
    Vector3 posBase;
    public Vector3 direction;
    Vector3 localDirection;
    public bool placed = false;
    float lastAngle;
    int sticking;
    
    void Start()
    {
        placed = false;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!placed)
            {

                placed = true;
                posBase = Input.mousePosition;
                return;
            }
            
            localDirection = (Input.mousePosition - posBase).normalized;
            direction = new Vector3(localDirection.x, 0, localDirection.y);
        }
        else
        {
            placed = false;
            direction = new Vector3();
        }
    }

    public float RotateEulers(float rotationSpeed)
    {
        float angle = Mathf.Asin(direction.x) / Mathf.PI * 180;

        if (direction.z < 0)
            if (angle < 0) angle = -180 - angle;
            else angle = 180 - angle;

        float eulers = angle - transform.rotation.eulerAngles.y;
        while (Mathf.Abs(eulers) > 180) eulers = -1 * Mathf.Sign(eulers) * (Mathf.Abs(eulers) - 180);
        if (Mathf.Abs(eulers) > rotationSpeed * Time.fixedDeltaTime) eulers = Mathf.Sign(eulers) * rotationSpeed * Time.fixedDeltaTime;
        if (Mathf.Abs(eulers) < 0.01f) return 0;
        if (lastAngle == -eulers)
        {
            sticking += 1;
            if (sticking > 2)
            {
                return 0;
            }
            lastAngle = eulers;
        }
        else
        {
            sticking = 0;
        }
        
        return eulers;
    }
}
