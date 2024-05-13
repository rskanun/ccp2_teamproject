using UnityEngine;

public class BossA : BossMonster
{
    [Header("보스 공격 데이터")]
    [SerializeField] private BossASkill laserAttack;

    // 보스 공격 데이터
    private float attackRotate;

    protected override void AttackedPlayer(GameObject target)
    {
        if (currentCooldown <= 0.0f)
        {
            laserAttack.OnShoot(attackRotate);

            // 다음 공격할 방향 설정
            attackRotate += 45;

            // 쿨타임 설정
            currentCooldown = MonsterData.AttackCooldown;
        }
    }
}