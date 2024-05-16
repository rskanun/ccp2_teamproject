using UnityEngine;

public class BossA : BossMonster
{
    [Header("보스 공격 데이터")]
    [SerializeField] private float laserCooldown;
    [SerializeField] private BossASkill laserAttack;

    // 보스 공격 데이터
    private float attackRotate;

    protected override void OnCastSkill()
    {
        if (CoolTime <= 0.0f)
        {
            laserAttack.OnShoot(attackRotate);

            // 다음 공격할 방향 설정
            attackRotate -= 45;

            // 쿨타임 설정
            CoolTime = laserCooldown;
        }
    }

    protected override void AttackedPlayer(GameObject target)
    {
        // 해당 보스는 공격 대신 스킬을 사용함
    }
}