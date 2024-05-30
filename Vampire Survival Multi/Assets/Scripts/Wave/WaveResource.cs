using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveResource : ScriptableObject
{
    [System.Serializable]
    private class WaveMonsterData
    {
        public GameObject monster;
        public int spawnCount;
    }

    [System.Serializable]
    private class Wave
    {
        public int time;
        public bool isBossWave;
        public List<WaveMonsterData> monsters;
    }

    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Wave";
    private const string FILE_PATH = "Assets/Resources/Option/Wave/WaveResource.asset";

    private static WaveResource _instance;
    public static WaveResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<WaveResource>("Option/Wave/WaveResource");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }

                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<WaveResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<WaveResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("웨이브 정보")]
    [SerializeField] 
    private List<Wave> waveDatas;

    public int MaxLevel
    {
        get { return waveDatas.Count; }
    }

    public int GetWaveTime(int waveLevel)
    {
        int index = waveLevel - 1;

        return waveDatas[index].time;
    }
    
    public List<GameObject> GetWaveMobs(int waveLevel)
    {
        int index = waveLevel - 1;

        if (0 <= index && index < waveDatas.Count)
        {
            Wave resultData = waveDatas[index];
            List<GameObject> result = WaveDataToList(resultData);

            return result;
        }

        return null;
    }

    private List<GameObject> WaveDataToList(Wave waveData)
    {
        if (waveData.isBossWave)
        {
            // 보스 웨이브일 경우 랜덤한 보스몹 리턴
            return GetBossMob(waveData);
        }
        else
        {
            // 일반 웨이브일 경우 모든 몬스터 리턴
            return GetNormalMob(waveData);
        }
    }

    private List<GameObject> GetBossMob(Wave waveData)
    {
        List<GameObject> spawnBossList = new List<GameObject>();

        if (waveData.monsters.Count > 0)
        {
            int randomIndex = Random.Range(0, waveData.monsters.Count);

            // 소환할 보스몹 정보
            WaveMonsterData bossData = waveData.monsters[randomIndex];

            int count = bossData.spawnCount;
            GameObject spawnMob = bossData.monster;

            for (int i = 0; i < count; i++)
            {
                spawnBossList.Add(spawnMob);
            }
        }

        return spawnBossList;
    }

    private List<GameObject> GetNormalMob(Wave waveData)
    {
        List<GameObject> spawnMobList = new List<GameObject>();

        // 웨이브에 생성될 몹을 리스트로 변환
        foreach (WaveMonsterData waveMonsterData in waveData.monsters)
        {
            int count = waveMonsterData.spawnCount;
            GameObject spawnMob = waveMonsterData.monster;

            for (int i = 0; i < count; i++)
            {
                spawnMobList.Add(spawnMob);
            }
        }

        return spawnMobList;
    }

    public bool IsBossWave(int waveLevel)
    {
        int index = waveLevel - 1;

        return waveDatas[index].isBossWave;
    }
}