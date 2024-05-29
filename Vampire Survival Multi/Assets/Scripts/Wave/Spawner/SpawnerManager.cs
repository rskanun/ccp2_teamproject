using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("스포너")]
    [SerializeField] private List<Spawner> spawners;
    [SerializeField] private PlayerChecker checker;

    // 해당 플레이어의 주위로부터 스폰 가능 여부
    public bool IsSpawnable
    {
        get
        {
            // 하나의 스포너라도 스폰 조건에 만족하면 스폰 가능함
            foreach (Spawner spawner in spawners)
            {
                // 해당 스포너가 맵 안에 있어야 함
                if (spawner.IsSpawnerInMap)
                {
                    // 일정 반경 안에 플레이어가 없고, 해당 방향의 가장 끝에 위치할 경우
                    if (checker.playerInArea == false || spawner.IsPlayerInArea == false)
                    {
                        // 해당 플레이어로부터 몬스터가 스폰 가능함
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public void SpawnMob(GameObject mob)
    {
        bool isSpawnable = false;
        Spawner spawner = null;

        // 스폰 가능한 스포너가 나올 때까지 반복
        while(isSpawnable == false)
        {
            int num = Random.Range(0, spawners.Count);

            spawner = spawners[num];
            isSpawnable = spawner.IsSpawnerInMap
                && (checker.playerInArea == false || spawner.IsPlayerInArea == false);
        }

        spawner.SpawnMob(mob);
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
            if (spawner.IsSpawnerInMap
                && (checker.playerInArea == false || spawner.IsPlayerInArea == false))
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.blue;
            }

            Gizmos.DrawWireCube(spawner.Pivot, spawner.SpawnArea);
        }
    }
}