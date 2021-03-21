using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillSpawner : MonoBehaviour
{
    [SerializeField] GameObject skillPrefabs;
    [SerializeField] Transform skillSpawnTransform;
    public void Spawn()
    {
        var obj = Instantiate(skillPrefabs);
        obj.transform.position = skillSpawnTransform.position;
    }
}
