using UnityEngine;

public class Player : MonoBehaviour
{
    // 해당 플레이어 옵션
    private PlayerData status;

    // 플레이어 공통 옵션
    private PlayerResource playerOption;
    private float curDuration;

    private void Start()
    {
        playerOption = PlayerResource.Instance;
    }

    private void Update()
    {
        if (curDuration > 0)
        {
            curDuration -= Time.deltaTime;
        }
    }

    public void InitPlayerData(PlayerData initData)
    {
        status = initData;
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
            status.HP -= Mathf.Abs(damage);
            curDuration = playerOption.NoDamageDuration;

            if (status.HP <= 0)
            {
                // hp 값이 0이하면 죽음 처리
                OnDead();
            }
        }
    }

    private void OnDead()
    {
        // 플레이어가 죽었을 때
    }

    public void OnKilled()
    {

    }
}