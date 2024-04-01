using UnityEngine;

public class Monster : MonoBehaviour
{
    // 기본 데이터
    [SerializeField]
    private MonsterData data;
    public MonsterData MonsterData { get { return data; } }

    // 몬스터 스테이터스
    private float _currentHP;
    private float _currentCooldown;

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

    private void Update()
    {
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }
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
        if (_currentCooldown <= 0)
        {
            float damage = data.STR; // 데미지 공식

            Player player = target.GetComponent<Player>();
            player.OnTakeDamage(damage);

            // 데미지 쿨타임 적용
            _currentCooldown = data.AttackCooldown;
        }
    }

    public void OnTakeDamage(float damage)
    {
        _currentHP -= Mathf.Abs(damage);
        Debug.Log(damage + " take damage");
        Debug.Log("remain HP: " + _currentHP);

        if (_currentHP <= 0)
        {
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        Destroy(gameObject);
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