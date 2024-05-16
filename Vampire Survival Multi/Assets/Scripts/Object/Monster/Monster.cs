using Photon.Pun;
using UnityEngine;

public class Monster : MonoBehaviourPun
{
    [Header("몬스터 데이터")]
    [SerializeField]
    private MonsterData _data;
    public MonsterData MonsterData { get { return _data; } }

    // 몬스터 스테이터스
    private float currentHP;
<<<<<<< Updated upstream
    protected float currentCooldown;
=======
>>>>>>> Stashed changes

    // 몬스터 유한 상태 기계
    private FSM _fsm;
    protected FSM fsm
    {
        get { return _fsm; }
    }

    private void Awake()
    {
        _fsm = new FSM(new ChaseState(this));
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
         if (currentCooldown > 0)
        {
            currentCooldown -= time;
        }
    }

    public void OnEnable()
    {
        // init stat
<<<<<<< Updated upstream
        currentHP = _data.HP;
=======
        currentHP = data.HP;

        // init fsm
        fsm.SetState(new ChaseState(this));
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            AttackedPlayer(target);
        }
    }

    protected virtual void AttackedPlayer(GameObject target)
    {
        float damage = _data.STR; // 데미지 공식
=======
            float damage = data.STR; // 데미지 공식
>>>>>>> Stashed changes

            Player player = target.GetComponent<Player>();
            player.OnTakeDamage(damage);
        }
    }

    public void OnTakeDamage(Player attacker, float damage)
    {
        // 공격 받았을 때
        float dmg = Mathf.Abs(damage);
        float def = _data.DEF;
        float lastDamage = dmg / (dmg + def) * dmg;

        photonView.RPC(nameof(TakeDamage), RpcTarget.MasterClient, attacker.photonView.ViewID, lastDamage);
<<<<<<< Updated upstream
    }

    [PunRPC]
    protected void TakeDamage(int attackerViewID, float lastDamage)
    {
        GameObject attackerChr = PhotonView.Find(attackerViewID).gameObject;
        Player attacker = attackerChr.GetComponent<Player>();

        currentHP -= lastDamage;
        photonView.RPC(nameof(AsyncHP), RpcTarget.Others, currentHP);

        if (currentHP <= 0)
        {
            OnDead(attacker);
        }
    }

    [PunRPC]
    protected void AsyncHP(float currentHP)
    {
        this.currentHP = currentHP;
=======
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
    }

    [PunRPC]
    protected void TakeDamage(int attackerViewID, float lastDamage)
    {
        GameObject attackerChr = PhotonView.Find(attackerViewID).gameObject;
        Player attacker = attackerChr.GetComponent<Player>();

        currentHP -= lastDamage;
        photonView.RPC(nameof(AsyncHP), RpcTarget.Others, currentHP);

        if (currentHP <= 0)
        {
            OnDead(attacker);
        }
    }

    [PunRPC]
    protected void AsyncHP(float currentHP)
    {
        this.currentHP = currentHP;
>>>>>>> 3dafc852bf63d9812eb4e4d163bb0288b895f612
    }

    [PunRPC]
    protected void TakeDamage(int attackerViewID, float lastDamage)
    {
        GameObject attackerChr = PhotonView.Find(attackerViewID).gameObject;
        Player attacker = attackerChr.GetComponent<Player>();

        currentHP -= lastDamage;
        photonView.RPC(nameof(AsyncHP), RpcTarget.Others, currentHP);

        if (currentHP <= 0)
        {
            OnDead(attacker);
        }
    }

    [PunRPC]
    protected void AsyncHP(float currentHP)
    {
        this.currentHP = currentHP;
    }

    protected void OnDead(Player killPlayer)
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
<<<<<<< HEAD
        Destroy(gameObject);

        // 몬스터 카운트 감소
        WaveData.Instance.OnKilledMob();
    }

    protected virtual int GetMonsterExp()
    {
        int exp = ExpResource.Instance.GetExp();
=======
        // 몬스터 제거
        photonView.RPC(nameof(DestroyMob), RpcTarget.All);

        // 경험치 획득
        int exp = GetMonsterExp();
        photonView.RPC(nameof(GetExp), RpcTarget.All, exp);
>>>>>>> 3dafc852bf63d9812eb4e4d163bb0288b895f612

        return exp;
    }

    [PunRPC]
    protected void GetExp(int exp)
    {
        GameData.Instance.AddExp(exp);
    }

    [PunRPC]
    protected void DestroyMob()
    {
<<<<<<< Updated upstream
        // 몬스터 제거
        photonView.RPC(nameof(DestroyMob), RpcTarget.All);

        // 경험치 획득
        int exp = GetMonsterExp();
        photonView.RPC(nameof(GetExp), RpcTarget.All, exp);
=======
        Destroy(gameObject);

        // 몬스터 카운트 감소
        WaveData.Instance.OnKilledMob();
    }

    protected virtual int GetMonsterExp()
    {
        int exp = ExpResource.Instance.GetExp();
>>>>>>> Stashed changes

        return exp;
    }

    [PunRPC]
    protected void GetExp(int exp)
    {
        GameData.Instance.AddExp(exp);
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
<<<<<<< Updated upstream
            _fsm.OnAction();
        }
    }

    public virtual void OnMove(Vector2 targetPos)
    {
        float speed = _data.MoveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed);
=======
            fsm.OnAction();
        }
>>>>>>> Stashed changes
    }
}