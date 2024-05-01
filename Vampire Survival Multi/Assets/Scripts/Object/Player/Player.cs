using UnityEngine;

public class Player : MonoBehaviour
{
    // 해당 플레이어 옵션
    private PlayerData playerData;

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
        if (playerData != null)
        {
            playerData.HP = playerData.MaxHP;
        }
    }

    private void Update()
    {
        if (curDuration > 0)
        {
            curDuration -= Time.deltaTime;
        }
    }

    public void InitPlayerData(PlayerData playerData)
    {
        Debug.Log("Init Data");
        this.playerData = playerData;

        // Reset HP
        playerData.HP = playerData.MaxHP;
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

            playerData.HP -= lastDamage;
            curDuration = playerOption.NoDamageDuration;

            if (playerData.HP <= 0)
            {
                // hp 값이 0이하면 죽음 처리
                OnDead();
            }
        }
    }

    public void OnAttack(Monster monster, float damage)
    {
        float lastDamage = monster.OnTakeDamage(this, damage);

        playerData.HP += lastDamage * playerData.LifeSteal;
    }

    private void OnDead()
    {
        // 게임 데이터에 플레이어 값 저장
        GameData gameData = GameData.Instance;

        gameData.AddDeadList(gameObject);

        // 플레이어 오브젝트 비활성화
        gameObject.SetActive(false);
    }  

    public void OnKilled()
    {

    }
}