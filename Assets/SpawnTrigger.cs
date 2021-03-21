using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent<Collider> evt;
    [SerializeField] string[] triggerTags;


    private void OnTriggerEnter(Collider other)
    {
        foreach (var tag in triggerTags)
        {
            if (other.CompareTag(tag))
                evt?.Invoke(other);
        }
    }
}
