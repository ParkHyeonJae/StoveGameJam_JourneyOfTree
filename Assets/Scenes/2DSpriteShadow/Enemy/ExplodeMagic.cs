using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMagic : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    Vector3 dir = Vector3.zero;
    private void Start()
    {
        dir = transform.position - Player.player.transform.position;
        dir = dir.normalized;
    }

    void Update()
    {
        transform.Translate(-dir * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
