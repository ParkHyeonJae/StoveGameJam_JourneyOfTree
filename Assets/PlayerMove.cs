using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
}
