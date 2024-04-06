using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("스포너")]
    [SerializeField] private List<Spawner> spawners;

    public void SpawnMonster()
    {
        // 주변 플레이어 탐지
        float distance = SpawnerResource.Instance.Distance;
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                
            }
        }
    }

    private List<Spawner> GetSpawnableList()
    {
        return null;
    }

    private void OnDrawGizmos()
    {
        SpawnerResource resource = SpawnerResource.Instance;

        // 플레이어 탐지 범위
        float distance = resource.Distance;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distance);

        // 스폰 가능한 스포너
        foreach(Spawner spawner in spawners)
        {
            if (spawner.PlayerInArea == false)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(spawner.Pivot, spawner.SpawnArea);
            }
        }
    }
}