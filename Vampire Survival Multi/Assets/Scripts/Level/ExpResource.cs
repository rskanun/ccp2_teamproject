using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

public class ExpResource : ScriptableObject
{
    private class ExpTable
    {
        private Dictionary<int, int> table;
        public List<int> Exps
        {
            get
            {
                List<int> list = new List<int>(table.Keys);

                // 순서 정렬
                list.Sort();

                return list;
            }
        }
        private int sum;
        public int MaxValue
        {
            get { return sum; }
        }

        public ExpTable()
        {
            table = new Dictionary<int, int>();
        }

        public void AddData(int exp, int probability)
        {
            sum += probability;
            table[exp] = sum;
        }

        public int GetChance(int exp)
        {
            if (table.ContainsKey(exp))
            {
                return table[exp];
            }

            return 0;
        }
    }

    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Level";
    private const string FILE_PATH = "Assets/Resources/Option/Level/ExpResource.asset";

    private static ExpResource _instance;
    public static ExpResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ExpResource>("Option/Level/ExpResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<ExpResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ExpResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("웨이브별 획득 경험치 확률표")]
    [SerializeField] private TextAsset expCSV;

    private Dictionary<int, ExpTable> _expTables;
    private Dictionary<int, ExpTable> ExpTables
    {
        set { _expTables = value; }
        get
        {
            if (_expTables == null)
            {
                _expTables = GetTables();
            }

            return _expTables;
        }
    }

    // 현재 경험치 정보
    private ExpTable curTable;

    private Dictionary<int, ExpTable> GetTables()
    {
        Dictionary<int, ExpTable> tables = new Dictionary<int, ExpTable>();

        if (expCSV != null)
        {
            StringReader sr = new StringReader(expCSV.text);

            string str;
            while((str = sr.ReadLine()) != null)
            {
                str = str.Split('#')[0];
                if (str.IsNullOrEmpty() == false)
                {
                    string[] strs = str.Split(",");

                    int waveLevel = int.Parse(strs[0]);
                    int getExp = int.Parse(strs[1]);
                    int probability = int.Parse(strs[2]);

                    if (tables.ContainsKey(waveLevel) == false)
                    {
                        // 새로운 테이블일 경우 새로 생성
                        tables[waveLevel] = new ExpTable();
                    }

                    tables[waveLevel].AddData(getExp, probability);
                }
            }
        }

        return tables;
    }

    [ContextMenu("Reload Table")]
    private void Reload()
    {
        ExpTables = GetTables();
    }

    public void SetWaveLevel(int waveLevel)
    {
        if (ExpTables.ContainsKey(waveLevel))
        {
            // 테이블에 있는 웨이브 레벨일 경우에만 갱신
            curTable = ExpTables[waveLevel];
        }
    }

    public int GetExp()
    {
        if (curTable != null)
        {
            // 확률에 따른 랜덤 경험치 지급
            int randomNum = Random.Range(1, curTable.MaxValue + 1);

            foreach (int exp in curTable.Exps)
            {
                if (curTable.GetChance(exp) >= randomNum)
                {
                    return exp;
                }
            }
        }

        return 0;
    }
}