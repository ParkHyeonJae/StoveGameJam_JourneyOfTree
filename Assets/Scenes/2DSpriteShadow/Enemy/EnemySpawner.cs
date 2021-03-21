using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Enemys;

    [SerializeField] int MaxSpawnCount = 3;
    float AccumulateTime = 0f;
    float AccumulateStoredTime = 0f;
    float AccumulateStoredCurTime = 0f;

    //private void OnEnable()
    //{
    //    StartCoroutine(OnUpdate());
    //}
    public void EnemySpawnStart()
    {
        StartCoroutine(OnUpdate());
    }

    private IEnumerator OnUpdate()
    {
        while (gameObject.activeInHierarchy)
        {
            AccumulateTime += Time.deltaTime;
            AccumulateStoredTime += Time.deltaTime;

            if (AccumulateTime >= 10)
            {
                var spawnCount = Random.Range(1, MaxSpawnCount);
                var playerPos = Player.player.transform.position;
                for (int i = 0; i < spawnCount; i++)
                {
                    var spawnObj = Enemys[Random.Range(0, Enemys.Length)];
                    var obj = Instantiate(spawnObj);
                    obj.transform.position = new Vector3(playerPos.x - 10f, playerPos.y, playerPos.z);
                }
                AccumulateTime = 0f;
            }

            yield return null;
        }
        yield return null;
    }
}
