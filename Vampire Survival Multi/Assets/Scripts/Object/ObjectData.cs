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
    }

    [SerializeField]
    private int _defensive;
    /***************************************************************
    * [ 방어력 (Defensive) ]
    * 
    * 오브젝트의 방어력 수치로 받는 데미지에 영향을 끼친다.
    * 방어력 1당 1의 데미지를 줄인다.
    ****************************************************************/
    public int DEF
    {
        get { return _defensive; }
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
    }
}