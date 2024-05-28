using Photon.Pun;
using UnityEngine;

public class LaserBoss : BossMonster
{
    [Header("보스 공격 데이터")]
    [SerializeField] private float laserCooldown;
    [SerializeField] private LaserBossSkill laserAttack;

    // 보스 공격 데이터
    private float cooldown;
    private float attackRotate;

    protected override void OnCastSkill()
    {
        if (cooldown <= 0.0f)
        {
            // 레이저 스킬 사용
            photonView.RPC(nameof(OnLaserShoot), RpcTarget.All);

            // 쿨타임 설정
            cooldown = laserCooldown;
        }
    }

    [PunRPC]
    private void OnLaserShoot()
    {
        laserAttack.OnShoot(attackRotate);

        // 다음 공격할 방향 설정
        attackRotate -= 45;
    }

    protected override void OnCooldown()
    {
        if (cooldown > 0.0f)
        {
            cooldown -= Time.deltaTime;
        }
    }

    protected override void AttackedPlayer(GameObject target)
    {
        // 해당 보스는 공격 대신 스킬을 사용함
    }
}