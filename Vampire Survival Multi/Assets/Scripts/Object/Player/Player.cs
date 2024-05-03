using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    // 플레이어 공통 옵션
    private PlayerResource playerOption;
    private float curDuration;

    private void Start()
    {
        playerOption = PlayerResource.Instance;
    }

    private void OnEnable()
    {
        // Reset HP
        playerData.HP = playerData.MaxHP;
    }

    private void Update()
    {
        if (curDuration > 0)
        {
            float time = Time.deltaTime;

            photonView.RPC(nameof(PassedTime), RpcTarget.All, time);
        }
    }

    [PunRPC]
    private void PassedTime(float time)
    {
        curDuration -= Time.deltaTime;
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 다른 오브젝트와의 상호작용에 의한 상태 처리
    ***************************************************************/

    public void OnTakeDamage(float damage)
    {
        // 피격받은 지 일정시간이 경과하면 데미지
        if (curDuration <= 0)
        {
            // 공격 받았을 때
            float dmg = Mathf.Abs(damage);
            float def = playerData.DEF;
            float lastDamage = dmg / (dmg + def) * dmg;

            TakeDamage(lastDamage);
        }
    }

    public void OnAttack(Monster monster, float damage)
    {
        float lastDamage = monster.OnTakeDamage(this, damage);
        float recoverHP = lastDamage * playerData.LifeSteal;

        HealHP(recoverHP);
    }

    [PunRPC]
    private void OnDead()
    {
        // 게임 데이터에 플레이어 값 저장
        GameData.Instance.AddDeadList(gameObject);

        // 플레이어 오브젝트 비활성화
        gameObject.SetActive(false);
    }  

    public void OnKilled()
    {

    }


    /***************************************************************
    * [ 체력 처리 ]
    * 
    * 회복 및 피해에 의한 체력 처리
    ***************************************************************/

    [PunRPC]
    private void TakeDamage(float damage)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            photonView.RPC(nameof(TakeDamage), RpcTarget.MasterClient, damage);

            return;
        }

        playerData.HP -= damage;

        photonView.RPC(nameof(UpdateHP), RpcTarget.Others, playerData.HP);
        photonView.RPC(nameof(AsyncNoDamageTime), RpcTarget.All);

        if (playerData.HP <= 0)
        {
            // hp 값이 0이하면 죽음 처리
            photonView.RPC(nameof(OnDead), RpcTarget.All);
        }
    }

    [PunRPC]
    private void AsyncNoDamageTime()
    {
        curDuration = playerOption.NoDamageDuration;
    }

    [PunRPC]
    private void HealHP(float recoverHP)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            photonView.RPC(nameof(HealHP), RpcTarget.MasterClient, recoverHP);

            return;
        }

        playerData.HP += recoverHP;

        photonView.RPC(nameof(UpdateHP), RpcTarget.Others, playerData.HP);
    }

    [PunRPC]
    private void UpdateHP(float currentHP)
    {
        playerData.HP = currentHP;
    }
}