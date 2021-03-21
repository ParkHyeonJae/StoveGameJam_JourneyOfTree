using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] Transform parent;
    public void GroundSpawn()
    {
        var obj = Instantiate(gameObject, parent);
        obj.transform.position = new Vector3(transform.position.x - 25f, transform.position.y, transform.position.z);
    }

    public void DestroyGround()
    {
        Destroy(gameObject, 10.0f);
    }
}
