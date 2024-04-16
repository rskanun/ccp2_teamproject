﻿using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class WaveData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Wave";
    private const string FILE_PATH = "Assets/Resources/Option/Wave/WaveData.asset";

    private static WaveData _instance;
    public static WaveData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<WaveData>("Option/Wave/WaveData");

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
                _instance = AssetDatabase.LoadAssetAtPath<WaveData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<WaveData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("현재 웨이브 정보")]
    [SerializeField]
    private bool _isRunning;
    public bool IsRunning
    {
        get { return _isRunning; }
        set { _isRunning = value; }
    }

    [ReadOnly]
    [SerializeField]
    private int _waveLevel;
    public int WaveLevel
    {
        get { return _waveLevel; }
    }

    public bool IsLastWave
    {
        get
        {
            WaveResource resource = WaveResource.Instance;

            return _waveLevel >= resource.MaxLevel;
        }
    }


    [ReadOnly]
    [SerializeField]
    private float _remainTime;
    public float RemainTime
    {
        get { return _remainTime; }
        set { _remainTime = value; }
    }

    [ReadOnly]
    [SerializeField]
    private int _mobCount;
    public int MobCount
    {
        get { return _mobCount; }
        set { _mobCount = value; }
    }

    // 소환할 몬스터 목록
    private Queue<GameObject> _waveMobs;

    public GameObject GetMob()
    {
        if (_waveMobs == null) InitData();

        if (_waveMobs.Count > 0)
        {
            return _waveMobs.Dequeue();
        }
        else
            return null;
    }

    private void InitData()
    {
        WaveResource resource = WaveResource.Instance;

        int startLevel = 1;

        _waveLevel = startLevel;
        _remainTime = resource.GetWaveTime(startLevel);
        _mobCount = resource.GetWaveMobs(startLevel).Count;

        // 소환할 몬스터 목록 추가
        List<GameObject> mobList = resource.GetWaveMobs(startLevel);

        _waveMobs = new Queue<GameObject>(mobList);
    }

    public void NextWave()
    {
        WaveResource resource = WaveResource.Instance;

        int maxLevel = resource.MaxLevel;

        if (maxLevel > _waveLevel)
        {
            // 웨이브 레벨 증가
            int currentLevel = ++_waveLevel;

            // 웨이브 정보 갱신
            _remainTime = resource.GetWaveTime(currentLevel);
            _mobCount += resource.GetWaveMobs(currentLevel).Count;

            // 소환할 몬스터 목록 추가
            List<GameObject> mobList = resource.GetWaveMobs(currentLevel);

            AddMobs(mobList);
        }
        else
        {
            // 최종 웨이브 이후엔 웨이브 종료
            _isRunning = false;
            _waveMobs = null;
        }
    }

    private void AddMobs(List<GameObject> addMobList)
    {
        if (_waveMobs != null && _waveMobs.Count > 0)
        {
            foreach (GameObject mob in addMobList)
            {
                _waveMobs.Enqueue(mob);
            }
        }
        else
        {
            _waveMobs = new Queue<GameObject>(addMobList);
        }
    }
}