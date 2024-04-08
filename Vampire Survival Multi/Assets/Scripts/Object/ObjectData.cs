using UnityEngine;

public class ObjectData : ScriptableObject
{
    [Header("오브젝트 스테이터스")]

    [SerializeField]
    private int _healthPoint;
    /***************************************************************
     * [ 체력 (Health Point) ]
     * 
     * 오브젝트의 처음 생성될 때의
     * 생명력 수치로 0이하로 떨어지면 죽는다.
     ***************************************************************/
    public int HP
    {
        get { return _healthPoint; }
        set
        {
            if (_maxHealthPoint < value) _healthPoint = _maxHealthPoint;
            else if (value < 0) _healthPoint = 0;
            else _healthPoint = value;
        }
    }

    [SerializeField]
    private int _maxHealthPoint;
    public int MaxHP
    {
        get { return _maxHealthPoint; }
        set
        {
            if (value <= 0) _maxHealthPoint = 1; // 최대체력의 최하수치는 1로 설정
            else
            {
                _maxHealthPoint = value;

                if (_healthPoint > _maxHealthPoint)
                {
                    // 최대체력보다 기본 체력이 높을 경우 기본 체력 수정
                    _healthPoint = _maxHealthPoint;
                }
            }
        }
    }

    [SerializeField]
    private int _strength;
    /***************************************************************
    * [ 근력 (Strength) ]
    * 
    * 오브젝트의 근력 수치로 물리 공격력에 영향을 끼친다.
    * 근력 1당 1의 데미지를 준다.
    ****************************************************************/
    public int STR
    {
        get { return _strength; }
        set
        {
            if (value < 0) _strength = 0;
            else _strength = value;
        }
    }

    [SerializeField]
    private int _agility;
    /***************************************************************
    * [ 민첩 (Agility) ]
    * 
    * 오브젝트의 민첩 수치로 이동속도에 영향을 끼친다.
    ****************************************************************/
    public int AGI
    {
        get { return _agility; }
        set
        {
            if (value < 0) _agility = 0;
            else _agility = value;
        }
    }

    [SerializeField]
    private int _moveSpeed;
    /***************************************************************
    * [ 이동속도 (Speed) ]
    * 
    * 오브젝트의 이동속도로 민첩 수치에 영향을 받는다.
    ****************************************************************/
    public virtual int MoveSpeed
    {
        get
        {
            if (_moveSpeed <= 0)
            {
                _moveSpeed = Mathf.RoundToInt(_agility * 1000f);
            }

            return _moveSpeed;
        }
    }
}