using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExplode : MonoBehaviour
{
    [SerializeField] Transform[] Explodes;

    [ContextMenu("Explode!")]
    public void RandomExlpode()
    {
        foreach (var explode in Explodes)
        {
            Rigidbody rb;
            if ((rb = explode.GetComponent<Rigidbody>()) == null)
                rb = explode.gameObject.AddComponent<Rigidbody>();

            if ((explode.GetComponent<BoxCollider>() == null))
                explode.gameObject.AddComponent<BoxCollider>();

            rb?.AddExplosionForce(Random.Range(10, 50), explode.position, 300f);
        }
    }
}
