using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private HealthUI hpBar;

    // 플레이어 스텟
    private PlayerStat stat;

    private void Start()
    {
        stat = PlayerStat.Instance;
    }

    public void InitStat()
    {
        stat.InitStat();

        hpBar.UpdateHP(stat.HP, stat.MaxHP);
    }

    public void OnTakeDamage(int damage)
    {
        // 공격 받았을 때
        stat.HP -= damage;
        hpBar.UpdateHP(stat.HP, stat.MaxHP);

        if (stat.HP <= 0)
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