using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerStat : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Objects/Player";
    private const string FILE_PATH = "Assets/Resources/Objects/Player/PlayerData.asset";

    private static PlayerStat _instance;
    public static PlayerStat Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<PlayerStat>("Objects/Player/PlayerData");

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
                _instance = AssetDatabase.LoadAssetAtPath<PlayerStat>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<PlayerStat>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("클래스 데이터")]
    [SerializeField]
    private ClassData _classData;
    public ClassData Class
    {
        get { return _classData; }
        set { _classData = value; }
    }

    [Header("플레이어 현재 스텟")]
    [SerializeField]
    private int _currentHP;
    public int MaxHP
    {
        get { return _classData.HP; }
    }
    public int HP
    {
        get { return _currentHP; }
        set
        {
            if (value < 0)
                _currentHP = 0;
            else if (value > MaxHP)
                _currentHP = MaxHP;
            else
                _currentHP = value;
        }
    }

    [SerializeField]
    private int _currentSTR;
    public int STR
    {
        get { return _currentSTR; }
        set
        {
            if (value < 0)
                _currentSTR = 0;
            else
                _currentSTR = value;
        }
    }

    [SerializeField]
    private int _currentDEF;
    public int DEF
    {
        get { return _currentDEF; }
        set
        {
            if (value < 0)
                _currentDEF = 0;
            else
                _currentDEF = value;
        }
    }

    [SerializeField]
    private int _currentAGI;
    public int AGI
    {
        get { return _currentAGI; }
        set
        {
            if (value < 0)
                _currentAGI = 0;
            else
                _currentAGI = value;
        }
    }

    public void InitStat()
    {
        _currentHP = _classData.HP;
        _currentSTR = _classData.STR;
        _currentDEF = _classData.DEF;
        _currentAGI = _classData.AGI;
    }
}