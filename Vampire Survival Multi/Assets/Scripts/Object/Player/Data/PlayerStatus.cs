using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerStatus : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Objects/Player";
    private const string FILE_PATH = "Assets/Resources/Objects/Player/PlayerStatus.asset";

    private static PlayerStatus _instance;
    public static PlayerStatus Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<PlayerStatus>("Objects/Player/PlayerStatus");

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
                _instance = AssetDatabase.LoadAssetAtPath<PlayerStatus>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<PlayerStatus>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("이벤트")]
    [SerializeField] private GameEvent hpEvent;

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
    private float _currentHP;
    public float HP
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

            // 이벤트 알림
            hpEvent.NotifyUpdate();
        }
    }

    [SerializeField]
    private float _currentMaxHP;
    public float MaxHP
    {
        get { return _currentMaxHP; }
        set
        {
            if (value <= 0)
                _currentMaxHP = 1;
            else
                _currentMaxHP = value;

            // 이벤트 알림
            hpEvent.NotifyUpdate();
        }
    }

    [SerializeField]
    private float _currentSTR;
    public float STR
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
    private float _currentDEF;
    public float DEF
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
    private float _currentAGI;
    public float AGI
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

    [Header("플레이어 좌표")]
    [SerializeField]
    private Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }
}