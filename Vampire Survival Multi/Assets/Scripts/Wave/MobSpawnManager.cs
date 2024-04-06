using System.Collections.Generic;
using UnityEngine;

public class MobSpawnManager : MonoBehaviour
{
    [Header("스폰 시간")]
    [SerializeField] 
    private float spawnDelay;
    private float curTime;

    [Header("몬스터 생성 범위")]
    [SerializeField] private Vector2 spawnArea;

    // 스폰 몬스터 목록
    private Queue<GameObject> mobs;

    // 참조 데이터
    private MapData mapData;
    private GameData gameData;

    private void Start()
    {
        mapData = MapData.Instance;
        gameData = GameData.Instance;
    }

    public void SetSpawnMobs(List<GameObject> waveMobs)
    {
        // Queue에 담기
        mobs = new Queue<GameObject>(waveMobs);
    }

    private void Update()
    {
        if (curTime <= 0)
        {
            // 몬스터 스폰
            GameObject spawnMob = mobs.Dequeue();
            SpawnMob(spawnMob);

            // 딜레이 적용
            curTime = spawnDelay;
        }
        else
            curTime -= Time.deltaTime;
    }

    private void SpawnMob(GameObject mob)
    {
        // 스폰 위치 설정
        // Vector2 spawnPos = GetMobSpawnPos();

        // Instantiate(mob, spawnPos, Quaternion.identity);
    }

    private Vector2 GetLeftPlayerPos()
    {
        Vector2 pos = mapData.MaxPos;

        foreach(PlayerData player in gameData.PlayerList)
        {
            Vector2 playerPos = player.Position;
            if (pos.x > playerPos.x)
            {
                pos = playerPos;
            }
        }

        return pos;
    }

    private Vector2 GetRightPlayerPos()
    {
        Vector2 pos = mapData.MinPos;

        foreach (PlayerData player in gameData.PlayerList)
        {
            Vector2 playerPos = player.Position;
            if (pos.x < playerPos.x)
            {
                pos = playerPos;
            }
        }

        return pos;
    }

    private Vector2 GetUpPlayerPos()
    {
        Vector2 pos = mapData.MinPos;

        foreach (PlayerData player in gameData.PlayerList)
        {
            Vector2 playerPos = player.Position;
            if (pos.y < playerPos.y)
            {
                pos = playerPos;
            }
        }

        return pos;
    }

    private Vector2 GetDownPlayerPos()
    {
        Vector2 pos = mapData.MaxPos;

        foreach (PlayerData player in gameData.PlayerList)
        {
            Vector2 playerPos = player.Position;
            if (pos.y > playerPos.y)
            {
                pos = playerPos;
            }
        }

        return pos;
    }
}