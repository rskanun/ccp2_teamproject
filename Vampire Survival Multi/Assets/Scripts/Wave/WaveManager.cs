using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("몬스터 소환 정보")]
    [SerializeField]
    private float spawnDelay;
    private float curTime;

    [Header("테스트 스포너")]
    [SerializeField]
    private Spawner testSpawner;

    // 참조 데이터
    private WaveData waveData;
    private GameData gameData;

    // 스포너 정보
    private int spawnPlayerIndex = 0;

    private void Start()
    {
        waveData = WaveData.Instance;
        gameData = GameData.Instance;
    }

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
        GameObject spawnMob = waveData.GetMob();

        testSpawner.TestSpawn(spawnMob);
    }
}