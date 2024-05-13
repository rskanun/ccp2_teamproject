using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossResource : ScriptableObject
{
    [System.Serializable]
    private class WaveBossData
    {
        public int waveLevel;
        public List<GameObject> bossList;
    }

    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Objects/Monster";
    private const string FILE_PATH = "Assets/Resources/Objects/Monster/BossResource.asset";

    private static BossResource _instance;
    public static BossResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<BossResource>("Objects/Monster/BossResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<BossResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<BossResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("웨이브별 보스 출몰 데이터")]
    [SerializeField] 
    private List<WaveBossData> waveBossList;
    private Dictionary<int, WaveBossData> waveBossDict = new Dictionary<int, WaveBossData>();

    private void OnValidate()
    {
        if (waveBossList != null && waveBossList.Count > 0)
        {
            ReloadData();
        }
    }

    [ContextMenu("Reload Data")]
    private void ReloadData()
    {
        foreach (WaveBossData data in waveBossList)
        {
            if (data.bossList != null && data.bossList.Count > 0)
            {
                int level = data.waveLevel;

                waveBossDict[level] = data;
            }
        }
    }

    public GameObject GetWaveBoss(int waveLevel)
    {
        List<GameObject> bossList = waveBossDict[waveLevel].bossList;
        
        // 랜덤 보스 뽑기
        if (bossList != null && bossList.Count > 0)
        {
            int randomIndex = Random.Range(0, bossList.Count);

            return bossList[randomIndex];
        }

        return null;
    }

    public bool IsBossWave(int waveLevel)
    {
        return waveBossDict.ContainsKey(waveLevel);
    }
}