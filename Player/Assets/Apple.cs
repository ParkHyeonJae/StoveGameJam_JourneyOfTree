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
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        End = Vector3.Distance(direction, transform.position) / Speed;
        direction = Vector3.Normalize(direction - transform.position);
    }
    private void Update()
    {
        Debug.Log(transform.position);
        transform.position += direction * Speed * Time.deltaTime;
        T += Time.deltaTime;
        if(T > End + 5f)
        {
            Destroy(this.gameObject);
        }
    }
}