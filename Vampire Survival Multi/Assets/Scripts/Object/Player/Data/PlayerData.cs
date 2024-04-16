using UnityEngine;

[CreateAssetMenu(menuName = "Game Object/Player/Player Data", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("플레이어 정보")]
    [SerializeField] 
    private bool _isPlaying;
    public bool IsPlaying
    {
        get { return _isPlaying; }
        set { _isPlaying = value; }
    }

    [ReadOnly]
    [SerializeField]
    private Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    [Header("클래스")]
    [SerializeField]
    private ClassData _classData;
    public ClassData Class
    {
        get { return _classData; }
    }

    [Header("현재 스텟")]
    [ReadOnly]
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
            _hpEvent.NotifyUpdate();
        }
    }

    [ReadOnly]
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
            _hpEvent.NotifyUpdate();
        }
    }

    [ReadOnly]
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

    [ReadOnly]
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

    [ReadOnly]
    [SerializeField]
    private float _currentAttackSpeed;
    public float AttackSpeed
    {
        get { return _currentAttackSpeed; }
        set
        {
            if (value <= 0)
                _currentAttackSpeed = 1;
            else
                _currentAttackSpeed = value;
        }
    }

    [ReadOnly]
    [SerializeField]
    private float _currentSpeed;
    public float Speed
    {
        get { return _currentSpeed; }
        set
        {
            if (value < 0)
                _currentSpeed = 0;
            else
                _currentSpeed = value;
        }
    }

    [ReadOnly]
    [SerializeField]
    private float _currentLifeSteal;
    public float LifeSteal
    {
        get { return _currentLifeSteal; }
        set
        {
            if (value < 0)
                _currentAttackSpeed = 0;
            else
                _currentAttackSpeed = value;
        }
    }

    [Header("이벤트")]
    [SerializeField]
    private GameEvent _hpEvent;
    public GameEvent HPEvent
    {
        get { return _hpEvent; }
    }

    public void InitData(ClassData playerClass)
    {
        // 직업 설정
        _classData = playerClass;

        // 스텟 초기화
        _currentMaxHP = playerClass.HP;
        _currentSTR = playerClass.STR;
        _currentDEF = playerClass.DEF;
        _currentSpeed = playerClass.Speed;
    }

    // 직업 선택 창을 거치지 않고 직업 할당
    public void TestInitData()
    {
        // 현재 직업을 토대로 스텟 초기화
        _currentMaxHP = _classData.HP;
        _currentSTR = _classData.STR;
        _currentDEF = _classData.DEF;
        _currentSpeed = _classData.Speed;
    }
}