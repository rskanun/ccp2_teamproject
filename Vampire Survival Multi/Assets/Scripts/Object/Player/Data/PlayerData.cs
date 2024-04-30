using Photon.Pun;
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
            if (value <= 0)
                _currentSTR = 1;
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
                _currentAttackSpeed = 0.1f;
            else
                _currentAttackSpeed = value;
        }
    }

    [ReadOnly]
    [SerializeField]
    private float _currentMoveSpeed;
    public float MoveSpeed
    {
        get { return _currentMoveSpeed; }
        set
        {
            if (value <= 0)
                _currentMoveSpeed = 0.1f;
            else
                _currentMoveSpeed = value;
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
                _currentLifeSteal = 0;
            else
                _currentLifeSteal = value;
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
        // 스텟 초기화
        _currentMaxHP = playerClass.HP;
        _currentSTR = playerClass.STR;
        _currentDEF = playerClass.DEF;
        _currentAttackSpeed = playerClass.AttackSpeed;
        _currentMoveSpeed = playerClass.MoveSpeed;
        _currentLifeSteal = playerClass.LifeSteal;
    }

    [PunRPC]
    public void UpdatePosition(Vector2 pos)
    {
        _position = pos;
    }
}