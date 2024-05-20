using Photon.Pun;
using UnityEngine;

public class LaserBoss : BossMonster
{
    [Header("보스 공격 데이터")]
    [SerializeField] private float laserCooldown;
    [SerializeField] private LaserBossSkill laserAttack;

    // 보스 공격 데이터
    private float attackRotate;

    protected override void OnCastSkill()
    {
        if (CoolTime <= 0.0f)
        {
            // 레이저 스킬 사용
            photonView.RPC(nameof(OnLaserShoot), RpcTarget.All);

            // 쿨타임 설정
            photonView.RPC(nameof(SetCoolTime), RpcTarget.All);
        }
    }

    [PunRPC]
    private void OnLaserShoot()
    {
        laserAttack.OnShoot(attackRotate);

        // 다음 공격할 방향 설정
        attackRotate -= 45;
    }

    [PunRPC]
    private void SetCoolTime()
    {
        CoolTime = laserCooldown;
    }

    protected override void AttackedPlayer(GameObject target)
    {
        // 해당 보스는 공격 대신 스킬을 사용함
    }
}