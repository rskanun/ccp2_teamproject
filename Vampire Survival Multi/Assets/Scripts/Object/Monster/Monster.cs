using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("몬스터 데이터")]
    [SerializeField]
    private MonsterData data;
    public MonsterData MonsterData { get { return data; } }

    // 몬스터 스테이터스
    private float _currentHP;

    // 몬스터 유한 상태 기계
    private FSM fsm;

    private void Awake()
    {
        fsm = new FSM();
    }

    public void OnEnable()
    {
        // init stat
        _currentHP = data.HP;

        // init fsm
        fsm.SetState(new ChaseState(this));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.AttackDistance);
    }

    /***************************************************************
    * [ 상태 제어 ]
    * 
    * 몬스터의 상태 제어
    ***************************************************************/

    public virtual void OnAttack(GameObject target)
    {
        float damage = data.STR; // 데미지 공식

        Player player = target.GetComponent<Player>();
        player.OnTakeDamage(damage);
    }

    public void OnTakeDamage(float damage)
    {
        _currentHP -= Mathf.Abs(damage);

        if (_currentHP <= 0)
        {
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        Destroy(gameObject);

        // 몬스터 카운트 감소
        WaveData.Instance.MobCount -= 1;

        // 경험치 획득
        int exp = ExpResource.Instance.GetExp();
        GameData.Instance.AddExp(exp);
    }

    /***************************************************************
    * [ 움직임 제어 ]
    * 
    * 몬스터의 움직임 제어
    ***************************************************************/

    private void FixedUpdate()
    {
        fsm.OnAction();
    }
}