using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/GameData.asset";

    private static GameData _instance;
    public static GameData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<GameData>("Option/GameData");

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
                _instance = AssetDatabase.LoadAssetAtPath<GameData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<GameData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("이벤트")]
    [SerializeField] private GameEvent expEvent;
    [SerializeField] private GameEvent levelUpEvent;

    [Header("참가 플레이어 목록")]
    [SerializeField]
    [ReadOnly]
    private List<GameObject> _playerList;
    public List<GameObject> PlayerList
    {
        get { return _playerList; }
    }

    [Header("레벨 정보")]
    [SerializeField]
    private int _exp;
    public int Exp
    {
        get { return _exp; }
    }

    [SerializeField]
    private int _requireExp;
    public int RequireExp
    {
        get { return _requireExp; }
    }

    [SerializeField]
    private int _level;
    public int Level
    {
        get { return _level; }
    }

    public void InitData(List<GameObject> players)
    {
        InitPlayer(players);
        InitLevel();
    }

    private void InitPlayer(List<GameObject> players)
    {
        _playerList = players;
    }

    private void InitLevel()
    {
        // Init Level
        int initLevel = 1;

        _level = initLevel;
        _exp = 0;
        _requireExp = LevelResource.Instance.GetRequireExp(initLevel);

        // 경험치 변경 이벤트
        expEvent.NotifyUpdate();
    }

    public void AddExp(int exp)
    {
        if (_requireExp < int.MaxValue)
        {
            _exp += exp;

            // 레벨 및 레벨업에 필요한 경험치량 조정
            while (_exp >= _requireExp)
            {
                // 레벨업
                _level++;
                _exp -= _requireExp;

                // 필요 경험치량 조정
                _requireExp = LevelResource.Instance.GetRequireExp(_level);

                // 레벨업 이벤트
                levelUpEvent.NotifyUpdate();
            }

            // 경험치 변경 이벤트
            expEvent.NotifyUpdate();
        }
    }
}