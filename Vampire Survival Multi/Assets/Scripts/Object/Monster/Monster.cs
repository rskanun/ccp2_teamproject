using Photon.Pun;
using UnityEngine;

public enum MonsterState
{
    Stun
}

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
    private float _coolTime;
    protected float CoolTime
    {
        get { return _coolTime; }
        set { _coolTime = value; }
    }

    // 몬스터 유한 상태 기계
    private FSM _fsm;
    protected FSM fsm
    {
        private set {  _fsm = value; }
        get { return _fsm; }
    }

    // 버프&디버프 목록


    private void Awake()
    {
        fsm = new FSM(new ChaseState(this));
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            OnCooldown();
            OnCastSkill();
        }
    }

    protected virtual void OnCastSkill()
    {
        // 일정 시간마다 발동하는 스킬
    }

    private void OnCooldown()
    {
        float time = Time.deltaTime;

        photonView.RPC(nameof(PassedCooldown), RpcTarget.All, time);
    }

    [PunRPC]
    protected void PassedCooldown(float time)
    {
         if (CoolTime > 0)
        {
            CoolTime -= time;
        }
    }

    public void OnEnable()
    {
        // init stat
        HP = _data.HP;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.AttackDistance);
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
        float damage = Stat.STR; // 데미지 공식

        Player player = target.GetComponent<Player>();
        player.OnTakeDamage(damage);
    }

    public void OnTakeDamage(Player attacker, float damage)
    {
        // 공격 받았을 때
        float dmg = Mathf.Abs(damage);
        float def = Stat.DEF;
        float lastDamage = dmg / (dmg + def) * dmg;

        photonView.RPC(nameof(TakeDamage), RpcTarget.MasterClient, attacker.photonView.ViewID, lastDamage);
    }

    [PunRPC]
    protected void TakeDamage(int attackerViewID, float lastDamage)
    {
        GameObject attackerChr = PhotonView.Find(attackerViewID).gameObject;
        Player attacker = attackerChr.GetComponent<Player>();

        HP -= lastDamage;
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
        // 몬스터 제거
        photonView.RPC(nameof(DestroyMob), RpcTarget.All);

        // 경험치 획득
        int exp = GetMonsterExp();
        photonView.RPC(nameof(GetExp), RpcTarget.All, exp);

        // 플레이어에게 킬 알림
        killPlayer.OnKilled();
    }

    [PunRPC]
    protected void DestroyMob()
    {
        Destroy(gameObject);

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
        float speed = Stat.MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed);
    }
}