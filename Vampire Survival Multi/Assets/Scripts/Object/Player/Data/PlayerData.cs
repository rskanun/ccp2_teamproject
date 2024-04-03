using UnityEngine;

[CreateAssetMenu(menuName = "Game Object/Player/Player Data", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
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

    [Header("이벤트")]
    [SerializeField]
    private GameEvent hpEvent;
}