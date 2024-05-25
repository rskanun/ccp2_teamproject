using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    [Header("플레이어 데이터")]
    [SerializeField] 
    private PlayerData _playerData;
    public PlayerData PlayerData
    {
        private set { _playerData = value; }
        get { return _playerData; }
    }

    [Header("이벤트")]
    [SerializeField] private GameEvent deadEvent;

    // 플레이어 버프
    private BuffManager buffManager = new BuffManager();
    private bool isInvisibleState = false;

    // 플레이어 공통 옵션
    private PlayerResource playerOption;
    private float curRegenDelay;
    private float curRegenCooldown;
    private float curNoHitTime;

    private void Start()
    {
        playerOption = PlayerResource.Instance;
    }

    private void OnEnable()
    {
        // Reset HP
        PlayerData.HP = PlayerData.MaxHP;
    }

    private void Update()
    {
        if (WaveData.Instance.IsRunning && PhotonNetwork.IsMasterClient)
        {
            OnCheckBuff();
            PassedTime();
        }
    }

    private void OnCheckBuff()
    {
        if (isInvisibleState == false && HasBuff(Buff.Invisible))
        {
            isInvisibleState = true;

            photonView.RPC(nameof(SetPlayerAlpha), RpcTarget.All, 0.7f);
        }
        else if (isInvisibleState && HasBuff(Buff.Invisible) == false)
        {
            isInvisibleState = false;

            photonView.RPC(nameof(SetPlayerAlpha), RpcTarget.All, 1.0f);
        }
    }

    [PunRPC]
    private void SetPlayerAlpha(float a)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        Color color = sprite.color;
        color.a = a;
        sprite.color = color;
    }

    private void PassedTime()
    {
        // 버프 쿨다운
        buffManager.BuffTimer(Time.deltaTime);

        if (curNoHitTime > 0) // 공격 쿨다운
            curNoHitTime -= Time.deltaTime;

        if (curRegenDelay > 0)
        {
            // 데미지를 안 받은 시간 쿨다운
            curRegenDelay -= Time.deltaTime;
        }
        else
        {
            if (curRegenCooldown <= 0)
            {
                // 일정 시간마다 체력 재생
                if (PlayerData.HP < PlayerData.MaxHP)
                {
                    // 플레이어의 체력이 닳은 경우에만 작동
                    HealHP(playerOption.RegenHP);

                    curRegenCooldown = playerOption.RegenCooltime;
                }
            }
            else
            {
                // 체력 재생 쿨다운
                curRegenCooldown -= Time.deltaTime;
            }
        }
    }

    [PunRPC]
    public void UpdatePlayerPos(Vector2 pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(UpdatePlayerPos), RpcTarget.Others, pos);
        }

        transform.position = pos;
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 다른 오브젝트와의 상호작용에 의한 상태 처리
    ***************************************************************/

    public void OnTakeDamage(float damage)
    {
        // 은신이 아니고, 피격받은 지 일정시간이 경과하면 데미지
        if (curNoHitTime <= 0 && HasBuff(Buff.Invisible) == false)
        {
            // 공격 받았을 때
            float dmg = Mathf.Abs(damage);
            float def = PlayerData.DEF;
            float lastDamage = dmg / (dmg + def) * dmg;

            TakeDamage(lastDamage);
        }
    }

    public void OnNormalAttack(Monster monster, float damage)
    {
        Debug.Log(damage);
        monster.OnTakeDamage(this, damage);

        // 최종 데미지에 따른 체력 회복
        float recoverHP = damage * PlayerData.LifeSteal;

        HealHP(recoverHP);
    }

    public void OnSkillAttack(Monster monster, float damage)
    {
        monster.OnTakeDamage(this, damage);
    }

    [PunRPC]
    private void OnDead()
    {
        // 게임 데이터에 플레이어 값 저장
        GameData.Instance.AddDeadList(gameObject);

        // 플레이어 오브젝트 비활성화
        gameObject.SetActive(false);

        if (PlayerData.Player.IsLocal)
        {
            LocalPlayerData.Instance.IsDead = true;

            // Notify Dead Event
            deadEvent.NotifyUpdate();
        }
    }

    public void OnKilled()
    {

    }


    /***************************************************************
    * [ 체력 처리 ]
    * 
    * 회복 및 피해에 의한 체력 처리
    ***************************************************************/

    private void TakeDamage(float damage)
    {
        PlayerData.HP -= damage;

        photonView.RPC(nameof(UpdateHP), RpcTarget.All, PlayerData.HP);
        photonView.RPC(nameof(AsyncNoDamageTime), RpcTarget.All);

        if (PlayerData.HP <= 0)
        {
            // hp 값이 0이하면 죽음 처리
            photonView.RPC(nameof(OnDead), RpcTarget.All);
        }
    }

    [PunRPC]
    private void AsyncNoDamageTime()
    {
        curNoHitTime = playerOption.NoDamageDuration;
        curRegenDelay = playerOption.RegentDelay;
        curRegenCooldown = 0;
    }

    public void HealHP(float recoverHP)
    {
        PlayerData.HP += recoverHP;

        photonView.RPC(nameof(UpdateHP), RpcTarget.Others, PlayerData.HP);
    }

    [PunRPC]
    private void UpdateHP(float currentHP)
    {
        PlayerData.HP = currentHP;
    }

    /***************************************************************
    * [ 스텟 증가 ]
    * 
    * 현재 플레이어의 일시적 스탯 증가
    ***************************************************************/

    public void SetBuffSTR(float increaseSTR)
    {
        photonView.RPC(nameof(UpdateBuffSTR), RpcTarget.MasterClient, increaseSTR);
    }

    [PunRPC]
    private void UpdateBuffSTR(float increaseSTR)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(UpdateBuffSTR), RpcTarget.Others, increaseSTR);
        }

        PlayerData.BuffSTR = increaseSTR;
    }

    /***************************************************************
    * [ 버프 ]
    * 
    * 현재 플레이어가 가지고 있는 이로운 효과 처리
    ***************************************************************/

    public void AddBuff(Buff buff, float duration)
    {
        buffManager.AddBuff(buff, duration);
    }

    public bool HasBuff(Buff buff)
    {
        return buffManager.HasBuff(buff);
    }
}