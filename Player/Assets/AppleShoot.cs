using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleShoot : MonoBehaviour
{
    public GameObject Apple;
    public void Shoot()
    {
        Instantiate(Apple, transform.position, Quaternion.identity);
    }
}