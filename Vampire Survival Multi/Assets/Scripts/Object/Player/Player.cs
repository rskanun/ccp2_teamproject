using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 스텟
    private PlayerStatus status;

    private void Start()
    {
        status = PlayerStatus.Instance;

        // HP 초기화
        InitHP();
    }

    private void InitHP()
    {
        status.HP = status.MaxHP;
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 다른 오브젝트와의 상호작용에 의한 상태 처리
    ***************************************************************/

    public void OnTakeDamage(float damage)
    {
        // 공격 받았을 때
        status.HP -= Mathf.Abs(damage);

        if (status.HP <= 0)
        {
            // hp 값이 0이하면 죽음 처리
            OnDead();
        }
    }

    private void OnDead()
    {
        // 플레이어가 죽었을 때
    }
}