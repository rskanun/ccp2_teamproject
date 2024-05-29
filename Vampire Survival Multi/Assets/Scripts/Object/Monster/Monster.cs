using Photon.Pun;
using UnityEngine;

public class Monster : MonoBehaviourPun
{
    [Header("몬스터 데이터")]
    [SerializeField]
    private MonsterData _data;
    public MonsterData Stat { get { return _data; } }

    // 몬스터 스테이터스
    private float _currentHP;
    protected float HP
    {
        get { return _currentHP; }
        set { _currentHP = value; }
    }

    // 몬스터 상태이상 목록
    private StatusEffectManager statusManager;

    // 몬스터 유한 상태 기계
    private FSM fsm;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            statusManager = new StatusEffectManager();
            fsm = new FSM(new ChaseState(this));
        }
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            OnCooldown();
            OnCastSkill();
            StatusEffectsTimer();
        }
    }

    protected virtual void OnCastSkill()
    {
        // 일정 시간마다 발동하는 스킬
    }

    protected virtual void OnCooldown()
    {
        // 스킬 쿨다운
    }

    private void StatusEffectsTimer()
    {
        float time = Time.deltaTime;

        statusManager.EffectTimer(time);
    }

    public void OnEnable()
    {
        // init stat
        HP = Stat.HP;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Stat.AttackDistance);
    }

    /***************************************************************
    * [ 상태 제어 ]
    * 
    * 몬스터의 상태 제어
    ***************************************************************/

    public void OnAttack(GameObject target)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            AttackedPlayer(target);
        }
    }

    protected virtual void AttackedPlayer(GameObject target)
    {
        if (statusManager.HasStatusEffect(StatusEffect.Stun) == false)
        {
            float damage = Stat.STR; // 데미지 공식

            Player player = target.GetComponent<Player>();
            player.OnTakeDamage(damage);
        }
    }

    public void OnTakeDamage(Player attacker, float damage)
    {
        // 공격 받았을 때
        float dmg = Mathf.Abs(damage);
        float def = Stat.DEF;
        float lastDamage = dmg / (dmg + def) * dmg;

        // 취약 상태일 경우 2배 데미지
        HP -= lastDamage * (statusManager.HasStatusEffect(StatusEffect.Weakness) ? 2 : 1);
        photonView.RPC(nameof(AsyncHP), RpcTarget.Others, HP);

        if (HP <= 0)
        {
            OnDead(attacker);
        }
    }

    [PunRPC]
    protected void AsyncHP(float currentHP)
    {
        HP = currentHP;
    }

    protected virtual void OnDead(Player killPlayer)
    {
        // 경험치 획득
        int exp = GetMonsterExp();
        photonView.RPC(nameof(GetExp), RpcTarget.All, exp);

        // 플레이어에게 킬 알림
        killPlayer.OnKilled();

        // 몬스터 제거
        photonView.RPC(nameof(DestroyMob), RpcTarget.All);
    }

    [PunRPC]
    protected void DestroyMob()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);

        // 몬스터 카운트 감소
        WaveData.Instance.OnKilledMob();
    }

    protected virtual int GetMonsterExp()
    {
        int exp = ExpResource.Instance.GetExp();

        return exp;
    }

    [PunRPC]
    protected void GetExp(int exp)
    {
        GameData.Instance.AddExp(exp);
    }

    public void AddEffect(StatusEffect type, float duration)
    {
        statusManager.AddEffect(type, duration);
    }

    /***************************************************************
    * [ 움직임 제어 ]
    * 
    * 몬스터의 움직임 제어
    ***************************************************************/

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            fsm.OnAction();
        }
    }

    public virtual void OnMove(Vector2 targetPos)
    {
        if (statusManager.HasStatusEffect(StatusEffect.Stun) == false)
        {
            float speed = Stat.MoveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed);
        }
    }
}