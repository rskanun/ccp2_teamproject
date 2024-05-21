using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

public class RewardResource : ScriptableObject
{
    private class RewardTable
    {
        public int commonDropRate;
        public int rareDropRate;
        public int epicDropRate;
        public int legendDropRate;
    }

    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Reward";
    private const string FILE_PATH = "Assets/Resources/Option/Reward/RewardResource.asset";

    private static RewardResource _instance;
    public static RewardResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<RewardResource>("Option/Reward/RewardResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<RewardResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<RewardResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("보상 확률표")]
    [SerializeField] private TextAsset waveRewardCSV;
    [SerializeField] private TextAsset bossRewardCSV;

    private Dictionary<int, RewardTable> waveRewardTable;
    private Dictionary<int, RewardTable> bossRewardTable;

    [ContextMenu("Reload Table")]
    private void OnValidate()
    {
        waveRewardTable = GetRewardTable(waveRewardCSV);
        bossRewardTable = GetRewardTable(bossRewardCSV);
    }

    private Dictionary<int, RewardTable> GetRewardTable(TextAsset csvFile)
    {
        Dictionary<int, RewardTable> tables = new Dictionary<int, RewardTable>();

        if (csvFile != null)
        {
            StringReader sr = new StringReader(csvFile.text);

            int sumRate = 0;
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                str = str.Split('#')[0];
                if (str.IsNullOrEmpty() == false)
                {
                    string[] strs = str.Split(",");

                    int waveLevel = int.Parse(strs[0]);
                    ItemGrade grade = (ItemGrade)Enum.Parse(typeof(ItemGrade), strs[1]);
                    int dropRate = int.Parse(strs[2]);

                    if (tables.ContainsKey(waveLevel) == false)
                    {
                        // 새로운 테이블일 경우 새로 생성
                        tables[waveLevel] = new RewardTable();

                        sumRate = 0;
                    }

                    // 해당 확률표에 확률 적용
                    sumRate += dropRate;
                    SetDropRate(tables[waveLevel], grade, sumRate);
                }
            }
        }

        return tables;
    }

    private void SetDropRate(RewardTable table, ItemGrade grade, int dropRate)
    {
        switch (grade)
        {
            case ItemGrade.Common:
                table.commonDropRate = dropRate;
                break;

            case ItemGrade.Rare:
                table.rareDropRate = dropRate;
                break;

            case ItemGrade.Epic:
                table.epicDropRate = dropRate;
                break;

            case ItemGrade.Legend:
                table.legendDropRate = dropRate;
                break;
        }
    }

    public ItemData GetWaveReward(int waveLevel)
    {
        RewardTable table = waveRewardTable[waveLevel];

        // 아이템 등급 뽑기
        ItemGrade grade = GetRandomGrade(table);

        // 해당 등급의 랜덤 아이템 리턴
        return ItemResource.Instance.GetRandomItem(grade);
    }

    public ItemData GetBossReward(int waveLevel)
    {
        RewardTable table = bossRewardTable[waveLevel];

        // 아이템 등급 뽑기
        ItemGrade grade = GetRandomGrade(table);

        // 해당 등급의 랜덤 아이템 리턴
        return ItemResource.Instance.GetRandomItem(grade);
    }

    private ItemGrade GetRandomGrade(RewardTable table)
    {
        int num = UnityEngine.Random.Range(0, table.legendDropRate);
        Debug.Log($"0 ~ {table.legendDropRate}: {num}");

        if (num < table.commonDropRate) return ItemGrade.Common;
        else if (num < table.rareDropRate) return ItemGrade.Rare;
        else if (num < table.epicDropRate) return ItemGrade.Epic;
        else return ItemGrade.Legend;
    }
}