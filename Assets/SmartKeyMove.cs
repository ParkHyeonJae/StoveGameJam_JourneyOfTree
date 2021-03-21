using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartKeyMove : MonoBehaviour
{
    void Update()
    {
        var v3 = Input.mousePosition;
        v3.z = 10.0f;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        transform.position = v3 + new Vector3(0, 0, 10);
    }
}