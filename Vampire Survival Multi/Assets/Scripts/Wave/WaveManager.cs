using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("몬스터 소환 정보")]
    [SerializeField]
    private float spawnDelay;
    private float curTime;

    [Header("참조 스크립트")]
    [SerializeField] private WaveUI ui;

    // 참조 데이터
    private WaveData waveData;
    private GameData gameData;

    // 스포너 정보
    private List<SpawnerManager> spawnerList;
    private Queue<SpawnerManager> spawnerSeq;

    // 웨이브 정보
    public bool IsWaveEnded
    {
        get
        {
            // 남은 시간이 없거나, 모든 몬스터를 죽였을 경우
            bool isTimerEnded = waveData.RemainTime <= 0;
            bool isKilledAllMob = waveData.MobCount <= 0;

            // 웨이브 종료
            return isTimerEnded || isKilledAllMob;
        }
    }

    public void UpdateTimer()
    {
        ui.UpdateTimer((int)waveData.RemainTime);
    }

    private void Start()
    {
        waveData = WaveData.Instance;
        gameData = GameData.Instance;
    }

    public void InitSpawner()
    {
        spawnerList = new List<SpawnerManager>();
        spawnerSeq = new Queue<SpawnerManager>();

        foreach (GameObject obj in gameData.PlayerList)
        {
            SpawnerManager spawner = obj.GetComponentInChildren<SpawnerManager>();

            if (spawner != null)
                spawnerList.Add(spawner);
        }
    }

    /***************************************************************
    * [ 몬스터 스폰 ]
    * 
    * 웨이브 진행에 따른 몬스터 스폰
    ***************************************************************/

    private void Update()
    {
        if (curTime <= 0)
        {
            SpawnMob();

            curTime = spawnDelay;
        }
        else
            curTime -= Time.deltaTime;
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
                // 스포너 순서가 전부 돌면 다시 재정렬
                if (spawnerSeq.Count <= 0)
                {
                    List<SpawnerManager> newList = SuffleSeq(spawnerList);

                    spawnerSeq = new Queue<SpawnerManager>(newList);
                }

                spawner = spawnerSeq.Dequeue();
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