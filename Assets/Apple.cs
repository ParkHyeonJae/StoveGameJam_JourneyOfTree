using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public Vector3 direction;
    public float Speed;
    public float T = 0;
    float End;
    private void Awake()
    {
        var v3 = Input.mousePosition;
        v3.z = 10.0f;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        direction = v3;
        End = Vector3.Distance(direction, transform.position) / Speed;
        direction = Vector3.Normalize(direction - transform.position);
    }
    private void Update()
    {
        //Debug.Log(transform.position);
        transform.position += direction * Speed * Time.deltaTime;
        T += Time.deltaTime;
        if(T > End + 5f)
        {
            Destroy(this.gameObject);
        }
    }
}