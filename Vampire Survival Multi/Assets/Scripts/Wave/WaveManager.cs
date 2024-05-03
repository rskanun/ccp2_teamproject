using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("몬스터 소환 정보")]
    [SerializeField]
    private float spawnDelay;
    private float curTime;

    // 참조 데이터
    private WaveData waveData;
    private GameData gameData;

    // 스포너 정보
    private Queue<SpawnerManager> _spawnerSeq = new Queue<SpawnerManager>();
    private Queue<SpawnerManager> SpawnerSeq
    {
        get
        {
            // 스포너 순서가 전부 돌면 다시 재정렬
            if (_spawnerSeq.Count <= 0)
            {
                List<SpawnerManager> newList = SuffleSeq(SpawnerList);

                _spawnerSeq = new Queue<SpawnerManager>(newList);
            }

            return _spawnerSeq;
        }
    }

    private List<SpawnerManager> _spawnerList = new List<SpawnerManager>();
    private List <SpawnerManager> SpawnerList
    {
        get
        {
            if (_spawnerList.Count <= 0)
            {
                // Init Spawner List
                foreach (GameObject obj in gameData.PlayerList)
                {
                    SpawnerManager spawner = obj.GetComponentInChildren<SpawnerManager>();

                    if (spawner != null)
                        _spawnerList.Add(spawner);
                }
            }

            return _spawnerList;
        }
    }

    private void Start()
    {
        waveData = WaveData.Instance;
        gameData = GameData.Instance;

        waveData.InitData();
    }

    /***************************************************************
    * [ 몬스터 스폰 ]
    * 
    * 웨이브 진행에 따른 몬스터 스폰
    ***************************************************************/

    private void Update()
    {
        if (waveData.IsRunning)
        {
            if (curTime <= 0)
            {
                SpawnMob();

                curTime = spawnDelay;
            }
            else
                curTime -= Time.deltaTime;
        }
    }

    private void SpawnMob()
    {
        // 스폰할 몹
        GameObject spawnMob = waveData.GetMob();

        if (spawnMob != null)
        {
            bool isSpawnable = false;
            SpawnerManager spawner = null;

            // 스폰 가능한 스포너가 나올 때까지 반복
            while(isSpawnable == false)
            {
                spawner = SpawnerSeq.Dequeue();
                isSpawnable = spawner.IsSpawnable;
            }
            
            // 몹 스폰
            spawner.SpawnMob(spawnMob);
        }
    }

    private List<T> SuffleSeq<T>(List<T> origin)
    {
        List<T> result = new List<T>(origin);

        int index = result.Count;
        while (index > 1)
        {
            index--;

            int num = Random.Range(0, index);
            T t = result[num];
            result[num] = result[index];
            result[index] = t;
        }

        return result;
    }
}